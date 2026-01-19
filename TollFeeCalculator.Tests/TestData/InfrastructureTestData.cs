namespace TollFeeCalculator.Tests.TestData;

public static class InfrastructureTestData
{
    public static IEnumerable<object[]> SwedenHolidays_ByYear_Cases()
    {
        // Known year in policy set (2026 exists)
        yield return new object[] { 2026, true };

        // Null year => empty
        yield return new object[] { null, false };

        // Not present year => empty
        yield return new object[] { 2025, false };
        yield return new object[] { 2030, false };
    }
}