using TollFeeCalculator.Infrastructure.Policies.Holidays;
using TollFeeCalculator.Tests.TestData;

namespace TollFeeCalculator.Tests.Infrastructure.Policies;

[TestClass]
public sealed class SwedenHolidaysTests
{
    [DataTestMethod]
    [DynamicData(nameof(InfrastructureTestData.SwedenHolidays_ByYear_Cases), typeof(InfrastructureTestData), DynamicDataSourceType.Method)]
    public void ByYear_Cases_ReturnExpected(int year, bool expectedAny)
    {
        var set = Holidays.ByYear(year);
        Assert.AreEqual(expectedAny, set.Count > 0);
        Assert.IsTrue(set.All(d => d.Year == year));
    }
}