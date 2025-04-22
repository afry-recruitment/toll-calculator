using Xunit;
using Xunit.Abstractions;

namespace TollFeeCalculator;

public class TollCalculatorTests(ITestOutputHelper testOutput)
{
    [Fact]
    public void Motorbikes_are_toll_free()
    {
        Assert.True(TollCalculator.IsTollFreeVehicle(new Motorbike()));
    }

    [Fact]
    public void Cars_are_not_toll_free()
    {
        Assert.False(TollCalculator.IsTollFreeVehicle(new Car()));
    }

    [Fact]
    public void Null_values_do_not_crash_the_toll_free_check()
    {
        TollCalculator.IsTollFreeVehicle(null);
    }
}