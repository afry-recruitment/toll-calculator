namespace TestTollCalculator;

[Collection("TestTollCalculatorCollection")]
public class TollCalculatorTest
{
    private readonly TestTollCalculatorFixture fixture;

    public TollCalculatorTest(TestTollCalculatorFixture fixture)
    {
        this.fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
    }

    [Fact]
    public void GetTollTestWithNoReg()
    {
        try
        {
            var i = new DateTime(2013, 04, 03, 09, 21, 12);
            var fee = fixture.feeService.GetTollFee("", "Car", new List<DateTime>() { i });
            
        }
        catch (Exception ex)
        {
            Assert.Equal("No reg number.", ex.Message);            
        }       
    }

    [Fact]
    public void GetTollTestWithNoDates()
    {
        try
        {
            var i = new DateTime(2013, 04, 03, 09, 21, 12);
            var fee = fixture.feeService.GetTollFee("TEST111", "Car", null);

        }
        catch (Exception ex)
        {
            Assert.Equal("No dates.", ex.Message);
        }        
    }

    [Fact]
    public void GetTollTestWithNoType()
    {
        try
        {
            var i = new DateTime(2013, 04, 03, 09, 21, 12);
            var fee = fixture.feeService.GetTollFee("TEST111", "", new List<DateTime>() { DateTime.Now });

        }
        catch (Exception ex)
        {
            Assert.Equal("No vehicle type.", ex.Message);
        }
    }

    public static IEnumerable<object[]> FreeDatesData =>
    new List<object[]>
    {
        new object[] { new DateTime(2013, 01, 01) },
        new object[] { new DateTime(2013, 03, 28) },
        new object[] { new DateTime(2013, 03, 29) },
        new object[] { new DateTime(2013, 04, 01) },
        new object[] { new DateTime(2013, 04, 30) },
        new object[] { new DateTime(2013, 05, 01) },
        new object[] { new DateTime(2013, 05, 08) },
        new object[] { new DateTime(2013, 05, 09) },
        new object[] { new DateTime(2013, 06, 05) },
        new object[] { new DateTime(2013, 06, 06) },
        new object[] { new DateTime(2013, 06, 21) },
        new object[] { new DateTime(2013, 11, 01) },
        new object[] { new DateTime(2013, 12, 24) },
        new object[] { new DateTime(2013, 12, 25) },
        new object[] { new DateTime(2013, 12, 26) },
        new object[] { new DateTime(2013, 12, 31) },
    };
    [Theory]
    [MemberData(nameof(FreeDatesData))]
    public void GetTollTestFreeDates(DateTime date)
    {
        var fee = fixture.feeService.GetTollFee("TEST111", "Car", new List<DateTime>() { date });
        Assert.Equal(0, fee);
    }

    public static IEnumerable<object[]> FreeVehiclesData =>
    new List<object[]>
    {
        new object[] { "Motorbike" },
        new object[] { "Tractor" },
        new object[] { "Emergency" },
        new object[] { "Diplomat" },
        new object[] { "Foreign" },
        new object[] { "Military" }
    };

    [Theory]
    [MemberData(nameof(FreeVehiclesData))]
    public void GetTollTestFreeVehicles(string vehicleType)
    {
        var fee = fixture.feeService.GetTollFee("TEST111", vehicleType, new List<DateTime> { DateTime.Now });
        Assert.Equal(0, fee);
    }

    public static IEnumerable<object[]> DatesMaxPeriodData =>
    new List<object[]>
    {
        new object[] { new List<DateTime>() {
            new DateTime(2013, 04, 03, 07, 21, 12),
            new DateTime(2013, 04, 03, 07, 23, 12) } }
    };

    [Theory]
    [MemberData(nameof(DatesMaxPeriodData))]
    public void GetTollTestMaxPeriod(List<DateTime> dates)
    {
        var fee = fixture.feeService.GetTollFee("TEST111", "Car", dates);
        Assert.Equal(18, fee);
    }

    public static IEnumerable<object[]> DatesMaxFeeData =>
    new List<object[]>
    {
        new object[] { new List<DateTime>() {
            new DateTime(2013, 04, 03, 09, 21, 12) ,
            new DateTime(2013, 04, 03, 07, 21, 12),
            new DateTime(2013, 04, 03, 07, 23, 12),
            new DateTime(2013, 04, 03, 06, 12, 12),
            new DateTime(2013, 04, 03, 08, 25, 12),
            new DateTime(2013, 04, 03, 15, 15, 12),
            new DateTime(2013, 04, 03, 16, 21, 12),
            new DateTime(2013, 04, 03, 15, 45, 12),
            new DateTime(2013, 04, 03, 17, 23, 12),
            new DateTime(2013, 04, 03, 18, 27, 12) } }
    };
    [Theory]
    [MemberData(nameof(DatesMaxFeeData))]
    public void GetTollTestMaxFee(List<DateTime> dates)
    {
        var fee = fixture.feeService.GetTollFee("TEST111", "Car", dates );
        Assert.Equal(60, fee);
    }
}