using TollFeeCalculator.Enterprise.Enums;
using TollFeeCalculator.Enterprise.Extensions;
using TollFeeCalculator.Tests.TestData;

namespace TollFeeCalculator.Tests.Enterprise.Extensions;

[TestClass]
public sealed class VehicleTypeExtensionsTests
{
    [DataTestMethod]
    [DynamicData(nameof(EnterpriseTestData.VehicleType_IsTollFree_TrueCases), typeof(EnterpriseTestData), DynamicDataSourceType.Method)]
    public void IsTollFree_TollFreeTypes_ReturnTrue(VehicleType type)
        => Assert.IsTrue(type.IsTollFree());

    [DataTestMethod]
    [DynamicData(nameof(EnterpriseTestData.VehicleType_IsTollFree_FalseCases), typeof(EnterpriseTestData), DynamicDataSourceType.Method)]
    public void IsTollFree_NonTollFreeTypes_ReturnFalse(VehicleType type)
        => Assert.IsFalse(type.IsTollFree());
}