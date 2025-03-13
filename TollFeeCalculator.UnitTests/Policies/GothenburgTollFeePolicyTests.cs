using System.Text.Json;
using TollFeeCalculator.Policies;

namespace TollFeeCalculator.UnitTests.Policies;
public class GothenburgTollFeePolicyTests
{
    private readonly GothenburgTollFeePolicy _policy = new();

    [Fact]
    public async Task GothenburgTollFeePolicy_VerifyBusinessRequirements_ShouldMatch()
    {
        var policy = new GothenburgTollFeePolicy();
        var json = JsonSerializer.Serialize(policy, new JsonSerializerOptions { WriteIndented = true });

        await Verify(json);
    }

    [Theory]
    [InlineData(2025, 12, 24)]
    [InlineData(1994, 4, 1)]
    [InlineData(2025, 3, 15)]

    public void IsTollFreeDate_DateIsAHolidayOrWeekend_ReturnsTrue(int year, int month, int day)
    {
        var result = _policy.IsTollFreeDate(new DateOnly(year, month, day));

        Assert.True(result);
    }

    [Fact]
    public void IsTollFreeDate_DateIsAWeekday_ReturnsFalse()
    {
        var result = _policy.IsTollFreeDate(new DateOnly(2025, 3, 13));

        Assert.False(result);
    }

    [Fact]
    public void IsTollFreeVehicleType_ForAllVehicleTypesExceptCar_ReturnsTrue()
    {
        foreach (var type in Enum.GetValues<VehicleType>())
        {
            if (type == VehicleType.Car)
                continue;

            var result = _policy.IsTollFreeVehicleType(type);

            Assert.True(result);
        }
    }
}