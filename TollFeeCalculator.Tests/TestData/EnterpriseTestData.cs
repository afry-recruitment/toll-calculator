using TollFeeCalculator.Enterprise.Enums;

namespace TollFeeCalculator.Tests.TestData;

public static class EnterpriseTestData
{
    public static IEnumerable<object[]> VehicleType_IsTollFree_TrueCases()
    {
        yield return new object[] { VehicleType.Motorbike };
        yield return new object[] { VehicleType.Tractor };
        yield return new object[] { VehicleType.Emergency };
        yield return new object[] { VehicleType.Diplomat };
        yield return new object[] { VehicleType.Foreign };
        yield return new object[] { VehicleType.Military };
    }

    public static IEnumerable<object[]> VehicleType_IsTollFree_FalseCases()
    {
        yield return new object[] { VehicleType.Car };
    }

    public static IEnumerable<object[]> FeeSchedule_GetFee_Cases()
    {
        // FeeRange descriptors are "HH:mm-HH:mm=fee" and parsed in tests
        var range = "06:00-07:00=13";

        // within range
        yield return new object[] { new[] { range }, "06:30", 13 };

        // equals From -> fee
        yield return new object[] { new[] { range }, "06:00", 13 };

        // equals To -> 0 because end exclusive
        yield return new object[] { new[] { range }, "07:00", 0 };

        // outside range
        yield return new object[] { new[] { range }, "05:59", 0 };

        // multiple ranges, match 2nd
        yield return new object[]
        {
            new[]
            {
                "06:00-06:30=8",
                "06:30-07:00=13",
            },
            "06:45",
            13
        };

        // overlapping ranges: implementation returns the first match
        yield return new object[]
        {
            new[]
            {
                "06:00-07:00=8",
                "06:30-08:00=18",
            },
            "06:45",
            8
        };
    }
}