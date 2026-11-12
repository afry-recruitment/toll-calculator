using TollCalculator.Models;
using TollCalculator.Services.Policies;

namespace TollCalculator.Tests.Policies;

public class DefaultTollFreeVehiclePolicyTests
{
    private readonly DefaultTollFreeVehiclePolicy _sut = new();

    [Fact]
    public void IsTollFree_Motorbike_ReturnsTrue()
    {
        Assert.True(_sut.IsTollFree(new Motorbike()));
    }

    [Fact]
    public void IsTollFree_Car_ReturnsFalse()
    {
        Assert.False(_sut.IsTollFree(new Car()));
    }

    [Fact]
    public void IsTollFree_NullVehicle_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.IsTollFree(null!));
    }
}