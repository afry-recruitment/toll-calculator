using TollFeeCalculator.Enterprise.ValueObjects;

namespace TollFeeCalculator.Enterprise.Policies.Schedules
{
    public static class GothenburgFeeSchedule
    {
        public static IReadOnlyList<FeeRange> Range { get; } = new List<FeeRange>
        {
            new(new TimeOnly(6, 0),  new TimeOnly(6, 30),  8),
            new(new TimeOnly(6, 30), new TimeOnly(7, 0),  13),
            new(new TimeOnly(7, 0),  new TimeOnly(8, 0),  18),
            new(new TimeOnly(8, 0),  new TimeOnly(8, 30), 13),
            new(new TimeOnly(8, 30), new TimeOnly(15, 0), 8),
            new(new TimeOnly(15, 0), new TimeOnly(15, 30), 13),
            new(new TimeOnly(15, 30), new TimeOnly(17, 0), 18),
            new(new TimeOnly(17, 0), new TimeOnly(18, 0), 13),
            new(new TimeOnly(18, 0), new TimeOnly(18, 30), 8),
        };
    }
}
