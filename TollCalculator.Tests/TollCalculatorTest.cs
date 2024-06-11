using TollCalculator.TollFee;

namespace TollCalculator.Tests;

public class TollCalculatorTest
{
    
    private static readonly TollFeeSpan[] TollFees = new TollFeeSpan[]
    {
        TollFeeSpan.Create(new TimeSpan(6, 0, 0), new TimeSpan(6, 30, 0), 8m).Value,
        TollFeeSpan.Create(new TimeSpan(6, 30, 0), new TimeSpan(7, 0, 0), 13m).Value,
        TollFeeSpan.Create(new TimeSpan(7, 0, 0), new TimeSpan(8, 0, 0), 18m).Value,
        TollFeeSpan.Create(new TimeSpan(8, 0, 0), new TimeSpan(15, 0, 0), 8m).Value,
        TollFeeSpan.Create(new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0), 13m).Value,
        TollFeeSpan.Create(new TimeSpan(15, 30, 0), new TimeSpan(17, 0, 0), 18m).Value,
        TollFeeSpan.Create(new TimeSpan(17, 0, 0), new TimeSpan(18, 0, 0), 13m).Value,
        TollFeeSpan.Create(new TimeSpan(18, 0, 0), new TimeSpan(23, 0, 0), 0m).Value,
    };

    private static readonly TollFeeTable TollFeeTable = TollFeeTable.Create(TollFees).Value;
    private static readonly TollCalculator Sut = new TollCalculator(TollFeeTable, 60, new Vehicle[]
        { new Vehicle("Motorbike"),
            new Vehicle("Tractor"),  
            new Vehicle("Emergency"),
            new Vehicle("Diplomat"), 
            new Vehicle("Foreign"),
            new Vehicle("Military")
        }, new []
        {
            new DateTime(2024, 5, 9, 0, 0, 0),
            new DateTime(2024, 6, 22, 0, 0, 0),
        }
    );
    
    [Fact]
    public void Max_fine_should_be_respected()
    {
        
        var vehicle = new Vehicle("Car");
        var dateArray = new []
        {
            new DateTime(2024, 5, 13, 6, 30, 0),
            new DateTime(2024, 5, 13, 7, 59, 0),
            new DateTime(2024, 5, 13, 14, 59, 0),
            new DateTime(2024, 5, 13, 15, 0, 0),
            new DateTime(2024, 5, 13, 16, 59, 0)
        };
        var result = Sut.GetTollFee(vehicle, dateArray);

        Assert.Equal(60, result);

    }
    
        [Fact]
    public void Multiple_fees_per_day_get_summed_up()
    {
        var vehicle = new Vehicle("Car");
        var dates = new[]
        {
            new DateTime(2024, 5, 13, 14, 59, 0),
            new DateTime(2024, 5, 13, 15, 0, 0),
            new DateTime(2024, 5, 13, 16, 59, 0),
        };
        Assert.Equal(39, Sut.GetTollFee(vehicle, dates));
    }

    [Fact]
    public void Fee_over_multiple_days_sum_up()
    {
        var vehicle = new Vehicle("Car");
        var dates = new[]
        {
            new DateTime(2024, 5, 13, 14, 59, 0),
            new DateTime(2024, 5, 13, 15, 0, 0),
            new DateTime(2024, 5, 13, 16, 59, 0),
            new DateTime(2024, 5, 14, 6, 30, 0),
            new DateTime(2024, 5, 14, 7, 59, 0),
            new DateTime(2024, 5, 14, 14, 59, 0),
            new DateTime(2024, 5, 14, 15, 0, 0),
            new DateTime(2024, 5, 14, 16, 59, 0),
        };
        Assert.Equal(99, Sut.GetTollFee(vehicle, dates));
    }
    

    [Fact]
    public void Only_highest_fee_applies_per_hour()
    {
        var vehicle = new Vehicle("Car");
        var dates = new[]
        {
            new DateTime(2024, 5, 13, 15, 0, 0),
            new DateTime(2024, 5, 13, 15, 30, 0),
        };
        Assert.Equal(18, Sut.GetTollFee(vehicle, dates));
    }

    [Theory]
    [InlineData("Motorbike")]
    [InlineData("Tractor")]
    [InlineData("Emergency")]
    [InlineData("Diplomat")]
    [InlineData("Foreign")]
    [InlineData("Military")]
    public void No_fee_on_fee_free_vehicles(string vehicleType)
    {
        var vehicle = new Vehicle(vehicleType);
        var dates = new[] { new DateTime(2024, 5, 13, 6, 0, 0) };
        Assert.Equal(0, Sut.GetTollFee(vehicle, dates));
    }

    [Theory]
    [MemberData(nameof(GetFeeFreeDates))]
    public void No_fee_on_fee_free_days(DateTime[] dates)
    {
        var vehicle = new Vehicle("Car");
        Assert.Equal(0, Sut.GetTollFee(vehicle, dates));
    }

    public static IEnumerable<object[]> GetFeeFreeDates()
    {
        yield return new object[] { new [] { new DateTime(2024, 5, 11, 17, 0, 0) } };  // weekend
        yield return new object[] { new [] { new DateTime(2024, 5, 12, 17, 0, 0) } };  // weekend
        yield return new object[] { new DateTime[] { new DateTime(2024, 5, 9, 17, 0, 0) } };   // holiday
    }
}