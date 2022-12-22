namespace TrafficToll.Internals.Services
{
    internal static class TollPassingsJoin
    {
        public static IEnumerable<IEnumerable<IEnumerable<DateTime>>> GetDateGroupsWithGroupedTollPassings(IEnumerable<DateTime> passings, TimeSpan validTollTime)
        {
            var passingsByDate = GroupPassingsByDayOfTheYearAndYear(passings);
            var groupedAfterTollTime = passingsByDate.Select(x => GroupPassingsByValidTollTime(x, validTollTime));
            return groupedAfterTollTime;
        }

        internal static IEnumerable<IEnumerable<DateTime>> GroupPassingsByValidTollTime(IEnumerable<DateTime> timeSpans, TimeSpan validTollTime)
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

        internal static IEnumerable<IEnumerable<DateTime>> GroupPassingsByDayOfTheYearAndYear(IEnumerable<DateTime> passings)
        {
            var groups = passings.GroupBy(x => x.Year.ToString() + x.DayOfYear.ToString());
            var passingGroups = groups.Select(x => x.ToArray());
            return passingGroups;
        }
    }
}
