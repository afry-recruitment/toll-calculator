using System.Globalization;
using TollFeeCalculator.Enterprise.Interfaces;
using TollFeeCalculator.Enterprise.Policies;
using TollFeeCalculator.Enterprise.ValueObjects;
using TollFeeCalculator.Tests.TestData;

namespace TollFeeCalculator.Tests.Enterprise.Policies;

[TestClass]
public sealed class FeeScheduleTests
{
    private sealed class FakeRangeProvider : IFeeRangeProvider
    {
        private readonly IReadOnlyList<FeeRange> _ranges;
        public FakeRangeProvider(FeeRange[] ranges) => _ranges = ranges;
        public IReadOnlyList<FeeRange> GetRange() => _ranges;
    }

    [DataTestMethod]
    [DynamicData(nameof(EnterpriseTestData.FeeSchedule_GetFee_Cases), typeof(EnterpriseTestData), DynamicDataSourceType.Method)]
    public void GetFee_Cases_ReturnExpected(string[] rangeDescriptors, string time, int expectedFee)
    {
        var ranges = rangeDescriptors.Select(ParseRange).ToArray();
        var parsedTime = TimeOnly.Parse(time, CultureInfo.InvariantCulture);

        IFeeSchedule schedule = new FeeSchedule(new FakeRangeProvider(ranges));
        Assert.AreEqual(expectedFee, schedule.GetFee(parsedTime));
    }

    private static FeeRange ParseRange(string descriptor)
    {
        // Format: "HH:mm-HH:mm=fee"
        var parts = descriptor.Split('=', 2, StringSplitOptions.TrimEntries);
        if (parts.Length != 2)
        {
            throw new FormatException($"Invalid FeeRange descriptor '{descriptor}'. Expected 'HH:mm-HH:mm=fee'.");
        }

        var times = parts[0].Split('-', 2, StringSplitOptions.TrimEntries);
        if (times.Length != 2)
        {
            throw new FormatException($"Invalid FeeRange descriptor '{descriptor}'. Expected 'HH:mm-HH:mm=fee'.");
        }

        var from = TimeOnly.Parse(times[0], CultureInfo.InvariantCulture);
        var to = TimeOnly.Parse(times[1], CultureInfo.InvariantCulture);
        var fee = int.Parse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture);

        return new FeeRange(from, to, fee);
    }
}