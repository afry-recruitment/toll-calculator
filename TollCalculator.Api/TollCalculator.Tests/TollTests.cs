using Application.Interfaces.GetTollFee;
using Library.UnitTests;
using TollCalculator.Domain.DTOs;
using TollCalculator.Infrastructure.Context;
using TollCalculator.Infrastructure.Repositories;

namespace TollCalculator.Tests;

public class TollCalculatorTests
{
    private readonly IGetTollFeeRepository _tollFeeRepository;
    private readonly InMemoryDbContext _dbContext;
    private readonly CancellationToken _ct;

    [Fact]
    public void TestTollFreeVehicle()
    {
        var helper = new TestHelper();
        var repo = helper.TestingInMemoryRepo();

        var tollFreeVehicles = helper.GetMockTollFreeVehicles();

        var mockDTO = new GetFeeByVehicleDTO
        {
            VehicleType = tollFreeVehicles.Select(x => x.Vehicle).First(),
            LicensPlate = "AAA111"
        };

        var res = repo.GetTollFee(mockDTO, _ct);

        Assert.Equal("Vehicle got charged 0 kr in toll", res.Result);
    }

    [Fact]
    public void TestTollFreeDay()
    {
        var helper = new TestHelper();
        var repo = helper.TestingInMemoryRepo();

        var mockDTO = new GetFeeByVehicleDTO
        {
            VehicleType = "Car",
            LicensPlate = "AAA111"
        };
        var res = repo.GetTollFee(mockDTO, _ct);

        Assert.Equal("Vehicle got charged 0 kr in toll", res.Result);
    }

    [Fact]
    public void TestValidTollTime()
    {
        var helper = new TestHelper();
        var repo = helper.TestingInMemoryRepo();

        var mockDTO = new GetFeeByVehicleDTO
        {
            VehicleType = "Car",
            LicensPlate = "AAA111"
        };
        helper.DeleteTollFreeDays();
        var res = repo.GetTollFee(mockDTO, _ct);
        var tollFee = helper.GetTollRates();
        var currentTime = TestHelper.ConvertTimeStringToInt(DateTime.Now.ToString("HH:mm:ss"));

        if(tollFee.Where(time => currentTime >= time.StartTime && currentTime <= time.EndTime).Select(x => x.Fee).FirstOrDefault() == 0)
        {
            Assert.Equal("Vehicle got charged 0 kr in toll", res.Result);
        } 
        else
        {
            Assert.StartsWith("Vechile's current toll charge for the day is", res.Result);
        }
            
    }
}