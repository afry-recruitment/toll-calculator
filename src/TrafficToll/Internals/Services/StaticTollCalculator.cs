using TrafficToll.Internals.ValueObjects;

namespace TrafficToll.Internals.Services
{
    internal static class StaticTollCalculator
    {
        public static int CalculateTotalSum(
            IEnumerable<DateTime> passings, 
            TimeSpan validTollTime, 
            IEnumerable<TollFeeSpan> tollFeeSpans
            )
        {
            int sum = 0;
            while (true)
            {
                var includedPassings = TakePassingsWithin1HourTimeRange(passings, validTollTime);
                var highestSpanFee = GetHighestFeeFromPassings(includedPassings, tollFeeSpans);
                sum += highestSpanFee;
                passings = passings.Where(x => !includedPassings.Contains(x)).ToArray();

                if (!passings.Any())
                    break;
            }

            return sum;
        }

        public static int GetHighestFeeFromPassings(IEnumerable<DateTime> includedPassings, IEnumerable<TollFeeSpan> tollFeeSpans)
        {
            var maxPrice = 0;
            TimeSpan span;
            foreach (var passing in includedPassings)
            {
                span = new TimeSpan(0, passing.Hour, passing.Minute, passing.Second, passing.Millisecond);
                var price = tollFeeSpans.Single(x => x.Start <= span && span < x.End).TollPrice;
                maxPrice = price > maxPrice ? price : maxPrice;
            }
            return maxPrice;
        }

        public static IEnumerable<DateTime> TakePassingsWithin1HourTimeRange(IEnumerable<DateTime> dateTimes, TimeSpan validTollTime)
        {
            var startTime = dateTimes.First();
            return dateTimes.Where(x => x >= startTime && x < startTime + validTollTime);
        }

        public static IEnumerable<DateTime> GetTollablePassings(IEnumerable<DateTime> passings, IEnumerable<(int year, int month, int date)> tollFreeDates)
        {
            var tollablePassings = PassingsWhereThereIsNoHoliday(passings, tollFreeDates);
            return PassingsWhichAreNotDuringWeekend(tollablePassings);
        }

        public static IEnumerable<DateTime> PassingsWhichAreNotDuringWeekend(IEnumerable<DateTime> passings)
        {
            return passings.Where(x => !(x.DayOfWeek == DayOfWeek.Saturday || x.DayOfWeek == DayOfWeek.Sunday));
        }

        public static IEnumerable<DateTime> PassingsWhereThereIsNoHoliday(IEnumerable<DateTime> passings, IEnumerable<(int year, int month, int date)> tollFreeDates)
        {
            return passings.Where(x => !tollFreeDates.Contains((x.Year, x.Month, x.Day)));
        }

        public static int CorrectWithMaximumDailyFee(int totalSum, int maximumDailyFee)
        {
            return totalSum < maximumDailyFee ? totalSum : maximumDailyFee;
        }
    }
}
