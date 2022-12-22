namespace TrafficToll.Internals.Services
{
    internal static class TollTimeCalculator
    {
        public static IEnumerable<IEnumerable<IEnumerable<DateTime>>> GroupTollPassings(IEnumerable<DateTime> passings, TimeSpan validTollTime)
        {
            



            var groupByDateThenTollTime = new List<IEnumerable<IEnumerable<DateTime>>>();
            //var groupedByDate = __GroupPassingsByDayOfTheYearAndYear(timeSpans);
            return groupByDateThenTollTime;
        }

        internal static IEnumerable<IEnumerable<IEnumerable<DateTime>>> GetDateGroupsWithGroupedTollPassings(IEnumerable<DateTime> passings, TimeSpan validTollTime)
        {
            var passingsByDate = __GroupPassingsByDayOfTheYearAndYear(passings);
            var groupedAfterTollTime = passingsByDate.Select(x => __GroupPassingsByValidTollTime(x, validTollTime));
            return groupedAfterTollTime;
        }

        internal static IEnumerable<IEnumerable<DateTime>> __GroupPassingsByValidTollTime(IEnumerable<DateTime> timeSpans, TimeSpan validTollTime)
        {
            var groupList = new List<IEnumerable<DateTime>>();

            while (true)
            {
                var startTime = timeSpans.First();
                var group = timeSpans.Where(x => x >= startTime && x < startTime + validTollTime);
                groupList.Add(group);

                timeSpans = timeSpans.Where(x => !group.Contains(x));

                if (!timeSpans.Any())
                    break;
            }

            return groupList;
        }

        internal static IEnumerable<IEnumerable<DateTime>> __GroupPassingsByDayOfTheYearAndYear(IEnumerable<DateTime> passings)
        {
            var groups = passings.GroupBy(x => x.Year.ToString() + x.DayOfYear.ToString());
            var passingGroups = groups.Select(x => x.ToArray());
            return passingGroups;
        }
    }
}
