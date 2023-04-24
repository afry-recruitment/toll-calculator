using PublicHoliday;

namespace TollCalculator.Tests.TestData
{
    public class PublicHolidayTestData : TheoryData<DateTime>
    {
        public PublicHolidayTestData()
        {
            var holidays = new SwedenPublicHoliday().PublicHolidays(DateTime.Now.Year);

            foreach (var date in holidays)
            {
                Add(date);
            }

            var week = new List<DateTime>
            {
                new DateTime(DateTime.Now.Year, 3, 1),
                new DateTime(DateTime.Now.Year, 3, 2),
                new DateTime(DateTime.Now.Year, 3, 3),
                new DateTime(DateTime.Now.Year, 3, 4),
                new DateTime(DateTime.Now.Year, 3, 5),
                new DateTime(DateTime.Now.Year, 3, 6),
                new DateTime(DateTime.Now.Year, 3, 7),
            };

            foreach (var date in week)
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                {
                    Add(date);
                }
            }
        }
    }
}
