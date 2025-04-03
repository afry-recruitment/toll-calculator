namespace CityTollFeeCalculator.Tolls.Policy;

public static class TollFreeDates
{
    private static readonly HashSet<(int Month, int Day)> dates = new()
    {
        (1, 1),
        (3, 28), (3, 29),
        (4, 1), (4, 30),
        (5, 1), (5, 8), (5, 9),
        (6, 5), (6, 6), (6, 21),
        (11, 1),
        (12, 24), (12, 25), (12, 26), (12, 31)
    };
    public static bool IsTollFreeDate(DateOnly dateTime) => dateTime.DayOfWeek == DayOfWeek.Saturday
        || dateTime.DayOfWeek == DayOfWeek.Sunday
        || dateTime.Month == 7
        || dates.Contains((dateTime.Month, dateTime.Day));
}
