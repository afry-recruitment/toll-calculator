using TollFeeCalculator.Core.Vehicles;

namespace TollFeeCalculator.Tests
{
    /// <summary>
    /// We need to check that the daily limit is applied correctly. The rules are:
    /// 
    /// 1.The maximum fee for one day is 60 SEK
    /// 
    /// 2. A vehicle should only be charged once an hour
    /// 2.1. In the case of multiple fees in the same hour period, the highest one applies.
    ///
    /// </summary>
    public class TollDailyLimitsTests
    {
        private readonly TollCalculator _calculator = new();


        [Fact]
        public void PassesWithinOneHour_AreChargedOnce()
        {
            var passes = new[]
            {
            new DateTime(2013, 1, 2, 7, 0, 0),    // 7:00 - 7:59 => 18 SEK
            new DateTime(2013, 1, 2, 7, 30, 0),   // 7:00 - 7:59 => 18 SEK
            new DateTime(2013, 1, 2, 8, 0, 0)     // 13 SEK,  exactly 60 minutes apart is still the same window
        };

            int fee = _calculator.GetTollFee(new Car(), passes);

            Assert.Equal(18, fee);
        }

        // (DateTime.Millisecond)
        // This code used the millisecond component of the DateTime, which is always 0
        // date.Millisecond - intervalStart.Millisecond = 0 and merged the whole day into a single window.
        // With a real TimeSpan difference these are 65 minutes
        // apart and must be charged as two separate passes (18 + 13 = 31).
        [Fact]
        public void PassesMoreThanOneHourApart_AreChargedSeparately()
        {
            var passes = new[]
            {
            new DateTime(2013, 1, 2, 7, 0, 0),   // 7:00 - 7:59 => 18 SEK
            new DateTime(2013, 1, 2, 8, 5, 0),   // 8:00 - 8:59 => 13 SEK, 65 minutes later
        };

            int fee = _calculator.GetTollFee(new Car(), passes);

            Assert.Equal(31, fee);   // It returned 18 now returned 31 = 18 + 13 ---> FIX-2
        }


        [Fact]
        public void PassesMoreThanOneHourApart_AreChargedSeparately_WithTimePassesDescendingOrder()
        {
            var passes = new[]
            {
                new DateTime(2013, 1, 2, 8, 5, 0),   // 8:00 - 8:59 => 13 SEK, 65 minutes before => -65 minutes => if (minutes <= WindowMinutes) 
                new DateTime(2013, 1, 2, 7, 0, 0),   // 7:00 - 7:59 => 18 SEK
                
            };

            int fee = _calculator.GetTollFee(new Car(), passes);

            Assert.Equal(31, fee);   // It returned 18 now returned 31 = 18 + 13 ---> FIX-4 The previous code was returning negative minutes when the dates were not in order, which caused incorrect fee calculations.
        }

        // (DateTime.Millisecond)
        // The old code used the 0-999 sub-second components, whose difference here is -400 ms,
        // so it saw "no time passed" although two hours separate the passes.
        [Fact]
        public void PassesTwoHoursApart_ChargeSeparately_EvenWithUnorderedMilliseconds()
        {
            var passes = new[]
            {
            new DateTime(2013, 1, 2, 7, 0, 0, 500),   // 18 SEK
            new DateTime(2013, 1, 2, 9, 0, 0, 100),   // 8 SEK, 120 minutes later
        };

            int fee = _calculator.GetTollFee(new Car(), passes);

            Assert.Equal(26, fee);   // It returned 18 now returned 31 = 18 + 8 ---> FIX-2
        }

        // (dead "else" branch never executed because of if(minutes <= 60) and the millisecond bug)
        // Three passes 61 minutes apart must sum their fees. With the old code every
        // pass collapsed into one window and the day cost the single highest fee.
        // It compares with dates[0] instead of the previous pass,
        // so it sees 61 minutes between the first and 122 mins between the last pass and sums correctly all three fees.
        // BUT intervalStart was never updated, so the next pass after this chain would be merged into the same window.
        [Fact]
        public void ThreePassesOverThreeHours_SumAllFees()
        {
            var passes = new[]
            {
            new DateTime(2013, 1, 2, 7, 5, 0),   // 18 SEK
            new DateTime(2013, 1, 2, 8, 6, 0),   // 13 SEK
            new DateTime(2013, 1, 2, 9, 7, 0),   //  8 SEK
        };

            Assert.Equal(39, _calculator.GetTollFee(new Car(), passes));   // It returned 18 ---> FIX-2
        }

        // intervalStart was never updated:
        // Every pass is within 60 minutes of the previous one the window is sliding,
        // so the chain 07:00 -> 07:50 -> 08:35 is one window and costs the
        // highest fee only.
        // DO NOT FORGET REQUIREMENT:
        //      A vehicle should only be charged once an hour AND
        //          In the case of multiple fees in the same hour period, the highest one applies.
        [Fact]
        public void ChainOfPassesWithin60MinutesOfPrevious_CountsAsOneWindow()
        {
            var passes = new[]
            {
            new DateTime(2013, 1, 2, 7, 0, 0),   // 18 SEK
            new DateTime(2013, 1, 2, 7, 50, 0),  // 18 SEK, 50 min after the previous
            new DateTime(2013, 1, 2, 8, 35, 0),  // 13 SEK, 45 min after the previous
        };

            Assert.Equal(18, _calculator.GetTollFee(new Car(), passes));

            // It returned 26 because intervalStart=date[0] was comparing the very first pass in the morning
            // with the rest of the passes during the day and in this case we catch the problem:
            // Pass 1: 7:00 -> +18
            // Pass 2: 7:50 -> +18 (merged into the same window, highest fee is still 18) MINUTES SINCE PREVIOUS PASS = 50
            // Pass 3: 8:35 -> +13 MINUTES SINCE PREVIOUS PASS = 95 (because first pass is still date[0] or Pass 1)
            // We should check with Pass 2 instead. This is the bug. It should be 45 minutes since previous pass, so it should merge
            // into the same window and the highest fee is still 18.
            // Result 26 but this is not correct.
            // First approach solution ---> FIX-3
        }

        // SPEC: the daily total is MAX 60 SEK 
        // Seven passes 61 minutes apart sum to 18+13+8+8+8+8+8 = 71 -> 60.
        [Fact]
        public void DailyFee_IsCappedAt60()
        {
            var passes = new[]
            {
            new DateTime(2013, 1, 2, 7, 5, 0),    // 18 SEK
            new DateTime(2013, 1, 2, 8, 6, 0),    // 13 SEK
            new DateTime(2013, 1, 2, 9, 7, 0),    //  8 SEK
            new DateTime(2013, 1, 2, 10, 8, 0),   //  8 SEK
            new DateTime(2013, 1, 2, 11, 9, 0),   //  8 SEK
            new DateTime(2013, 1, 2, 12, 10, 0),  //  8 SEK
            new DateTime(2013, 1, 2, 13, 11, 0),  //  8 SEK
        };

            Assert.Equal(60, _calculator.GetTollFee(new Car(), passes));
            // old code returned 18 ---> The milliseconds FIX-1 solves it. Before if(minutes<=60) was always true because of the millisecond bug, so it merged all seven passes into one window and returned the highest fee of 18.
        }

        [Fact]
        public void DailyFee_IsCappedAt60_WithTimePassesDisorderedAndDescending()
        {
            var passes = new[]
            {
                new DateTime(2013, 1, 2, 8, 6, 0),    // 13 SEK 
                new DateTime(2013, 1, 2, 11, 9, 0),   //  8 SEK 
                new DateTime(2013, 1, 2, 10, 8, 0),   //  8 SEK
                new DateTime(2013, 1, 2, 9, 7, 0),    //  8 SEK
                new DateTime(2013, 1, 2, 13, 11, 0),  //  8 SEK 
                new DateTime(2013, 1, 2, 12, 10, 0),  //  8 SEK
                new DateTime(2013, 1, 2, 7, 5, 0),    // 18 SEK
            };

            Assert.Equal(60, _calculator.GetTollFee(new Car(), passes));
            // old code returned 39 ---> FIX-4 The previous code was returning negative minutes when the dates were not in order, which caused incorrect fee calculations.

        }
    }
}
