using TollFeeCalculator.Application.Services;
using TollFeeCalculator.Enterprise.Entities;
using TollFeeCalculator.Tests.TestData;

namespace TollFeeCalculator.Tests.Application.Services;

[TestClass]
public sealed class VehicleServiceTests
{
    [TestMethod]
    public void IsTollFree_Null_ReturnsFalse()
    {
        var svc = new VehicleService();
        Assert.IsFalse(svc.IsTollFree(null!));
    }

    [DataTestMethod]
    [DynamicData(nameof(ApplicationTestData.VehicleService_IsTollFree_Cases), typeof(ApplicationTestData), DynamicDataSourceType.Method)]
    public void IsTollFree_Cases_ReturnExpected(Vehicle vehicle, bool expected)
    {
        var svc = new VehicleService();
        Assert.AreEqual(expected, svc.IsTollFree(vehicle));
    }
}