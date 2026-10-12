using TollFeeCalculator.Core.Vehicles;

namespace TollFeeCalculator.Tests
{
    /// <summary>
    /// Check every boundary of the hourly fee table. 
    /// The expected values describe the intended rules, not the behaviour 
    /// of the original if/else chain:
    /// 
    ///   - 09:15 FAILED -> a gap (the old chain only matched minute >= 30 and <=59 between 08:00 and 14:59) and is charged 0 instead of 8.
    ///   
    ///   - 15:29 and 15:31, change it to something easier to read that fulfills the requirements as well.
    ///   
    /// </summary>
    public class TollPayingFeeTests
    {
        private readonly TollCalculator _calculator = new();

        [Theory]
        // [hour, minute, expected fee in SEK]
        [InlineData(6, 0, 8)]     //OK
        [InlineData(6, 29, 8)]    //OK
        [InlineData(6, 30, 13)]   //OK
        [InlineData(6, 59, 13)]   //OK
        [InlineData(7, 0, 18)]    //OK
        [InlineData(7, 59, 18)]   //OK
        [InlineData(8, 0, 13)]    //OK
        [InlineData(8, 29, 13)]   //OK
        [InlineData(8, 30, 8)]    //OK

        [InlineData(9, 15, 8)]    // FIX-1: failed at 9:15 WOW found a gap in the 08:00-14:59 slot -> first 30 minutes of 09:00-14:59 slot must return 8 SEK but returned 0.
        [InlineData(9, 20, 8)]
        [InlineData(10, 15, 8)]   // FIX-1: failed at 10:15, 11:20, 12:25, 13:26 -> first 30 minutes of 09:00-14:59 slot must return 8 SEK but returned 0.
        [InlineData(11, 20, 8)]
        [InlineData(12, 25, 8)]
        [InlineData(13, 26, 8)]

        [InlineData(14, 59, 8)]   //OK

        [InlineData(15, 0, 13)]   // IMPROVE-1: confusing logic in if-else, it works but is hard to read.
        [InlineData(15, 29, 13)]
        [InlineData(15, 30, 18)]
        [InlineData(15, 31, 18)]
        [InlineData(15, 59, 18)]

        [InlineData(16, 59, 18)]    //OK
        [InlineData(17, 0, 13)]     //OK
        [InlineData(17, 59, 13)]    //OK
        [InlineData(18, 0, 8)]      //OK
        [InlineData(18, 29, 8)]     //OK
        [InlineData(18, 30, 0)]     //OK
        [InlineData(23, 59, 0)]     //OK
        [InlineData(5, 59, 0)]      //OK
        public void CalculateTollFeePerHour(int hour, int minute, int expectedFee)
        {
            var time = new DateTime(2013, 1, 2, hour, minute, 0);

            Assert.Equal(expectedFee, _calculator.GetTollFee(time, new Car()));
        }
    }
}
