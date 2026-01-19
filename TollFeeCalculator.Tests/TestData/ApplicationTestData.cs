using TollFeeCalculator.Application.Options;
using TollFeeCalculator.Enterprise.Entities;
using TollFeeCalculator.Enterprise.Enums;

namespace TollFeeCalculator.Tests.TestData;

public static class ApplicationTestData
{
    public static IEnumerable<object[]> TimeOnlyExtensions_ChunkByChargeWindow_Cases()
    {
        yield return new object[] { Array.Empty<string>(), 60, 0 };

        yield return new object[] { new[] { "06:00", "06:59" }, 60, 1 };

        // difference exactly equals 60 => new chunk
        yield return new object[] { new[] { "06:00", "07:00" }, 60, 2 };

        // difference just over 60 => new chunk
        yield return new object[] { new[] { "06:00", "07:01" }, 60, 2 };

        // unsorted list
        yield return new object[] { new[] { "07:30", "06:00", "06:30" }, 60, 2 };

        // multiple chunks
        yield return new object[] { new[] { "06:00", "07:00", "07:01" }, 0, 2 };
    }

    public static IEnumerable<object[]> VehicleService_IsTollFree_Cases()
    {
        yield return new object[] { new Vehicle(VehicleType.Car), false };
        yield return new object[] { new Vehicle(VehicleType.Motorbike), true };
    }

    public static IEnumerable<object[]> CalendarService_IsTollFreeAsync_WeekendCases()
    {
        yield return new object[] { new TollFeeOptions { TollFreeWeekends = true }, new DateTime(2026, 1, 3), true };  // Saturday
        yield return new object[] { new TollFeeOptions { TollFreeWeekends = true }, new DateTime(2026, 1, 4), true };  // Sunday
        yield return new object[] { new TollFeeOptions { TollFreeWeekends = false }, new DateTime(2026, 1, 3), false }; // Saturday, TollFreeWeekends disabled
    }

    public static IEnumerable<object[]> TollFeeService_GetFee_EdgeCases()
    {
        // null dateTimes => returns 0 fee result model (no exception)
        yield return new object[]
        {
            null!,
            new Vehicle(VehicleType.Car),
            new TollFeeOptions(),
            0
        };

        // empty list => returns 0 fee result model (no exception)
        yield return new object[]
        {
            Array.Empty<DateTime>(),
            new Vehicle(VehicleType.Car),
            new TollFeeOptions(),
            0
        };
    }

    public static IEnumerable<object[]> TollFeeService_GetFee_NormalAndRulesCases()
    {
        // Fee schedule provides fees by range.
        // CalendarService toll-free false, VehicleService toll-free false.

        // Single passage day
        yield return new object[]
        {
            new[] { new DateTime(2026, 1, 2, 6, 15, 0) },
            new TollFeeOptions { ChargeWindowMinutes = 60, MaxDailyFee = 60 },
            8
        };

        // 6:15(8) + 7:10 (18) => 55 mins => same chunk => 18
        yield return new object[]
        {
            new[] { new DateTime(2026, 1, 2, 6, 15, 0), new DateTime(2026, 1, 2, 7, 10, 0) },
            new TollFeeOptions { ChargeWindowMinutes = 60, MaxDailyFee = 60 },
            18
        };

        // Two windows => sum
        yield return new object[]
        {
            new[]
            {
                new DateTime(2026, 1, 2, 6, 15, 0), // 8
                new DateTime(2026, 1, 2, 8, 40, 0)  // 8 (08:30-15:00 => 8) and >60 mins from 6:15 => new chunk
            },
            new TollFeeOptions { ChargeWindowMinutes = 60, MaxDailyFee = 60 },
            16
        };

        // Max daily fee cap hit
        yield return new object[]
        {
            new[]
            {
                new DateTime(2026, 1, 2, 7, 0, 0),   // 18
                new DateTime(2026, 1, 2, 9, 0, 0),   // 8
                new DateTime(2026, 1, 2, 15, 30, 0), // 18
                new DateTime(2026, 1, 2, 17, 0, 0),  // 13
                new DateTime(2026, 1, 2, 18, 0, 0),  // 8
            },
            new TollFeeOptions { ChargeWindowMinutes = 60, MaxDailyFee = 60 },
            60
        };

        // Multiple days: totals sum between days
        yield return new object[]
        {
            new[]
            {
                new DateTime(2026, 1, 2, 6, 15, 0), // 8
                new DateTime(2026, 1, 3, 7, 10, 0), // would be 18 but weekend should be toll-free if enabled; this case assumes weekend disabled below
            },
            new TollFeeOptions { TollFreeWeekends = false, ChargeWindowMinutes = 60, MaxDailyFee = 60 },
            26
        };
    }
}