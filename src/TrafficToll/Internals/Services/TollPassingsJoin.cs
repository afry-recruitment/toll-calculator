namespace TrafficToll.Internals.Services
{
    internal static class TollPassingsJoin
    {
        public static IEnumerable<IEnumerable<IEnumerable<DateTime>>> GetDateGroupsWithGroupedTollPassings(IEnumerable<DateTime> passings, TimeSpan validTollTime)
        {
            //var passingsByDate = GroupPassingsByDayOfTheYearAndYear(passings);
            //var groupedAfterTollTime = passingsByDate.Select(x => GroupPassingsByValidTollTime(x, validTollTime));
            //var passingsByDate = GroupPassingsByValidTollTime(passings, validTollTime);
            //var groupedAfterTollTime = passingsByDate.Select(x => GroupPassingsByValidTollTime(x, validTollTime));
            //return groupedAfterTollTime.ToArray();
            throw new NotImplementedException();
        }

        internal static IEnumerable<IEnumerable<DateTime>> GroupPassingsByValidTollTime(IEnumerable<DateTime> dateTimes, TimeSpan validTollTime)
        {
            var groupList = new List<IEnumerable<DateTime>>();

            while (true)
            {
                var startTime = dateTimes.First();
                var group = dateTimes.Where(x => x >= startTime && x < startTime + validTollTime);
                
                
                groupList.Add(group);

                dateTimes = dateTimes.Where(x => !group.Contains(x)).ToArray();

                if (!dateTimes.Any())
                    break;
            }

            return groupList;
        }

        internal static DateTime[][] GroupPassingsByDayOfTheYearAndYear(IEnumerable<DateTime> passings)
        {
            var groups = passings.GroupBy(x => x.Year.ToString() + x.DayOfYear.ToString());
            var passingGroups = groups.Select(x => x.ToArray());
            return passingGroups.ToArray();
        }
    }
}
