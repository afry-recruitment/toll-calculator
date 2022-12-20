using FluentAssertions;
using System.Text.Json;
using toll_calculator.enums;
using toll_calculator.models;
using toll_calculator.repository;
using Xunit;
using Xunit.Abstractions;

namespace toll_calculator.tests;


public class TrafficTollRepositoryTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    //Fees will differ between 8 SEK and 18 SEK, depending on the time of day

    public TrafficTollRepositoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Get_traffic_toll_from_repository_assert_no_exception()
    {
        var trafficTollSpec = TrafficTollSpecificationRepository.GetTrafficTollSpecification();
        trafficTollSpec.ValidFrom.Year.Should().Be(2013);
    }

    [Fact]
    public void Create_traffic_toll_for_2013()
    {
        var trafficToll2013 = new TrafficTollSpecification(
            validFrom: new DateTime(2013, 1, 1),
            validUntil: new DateTime(2013, 12, 31),
            maximumDailyFee: 60,
            validTollTime: new TimeSpan(0,1,0),
            dailyTollTimePrizes: new[]
            {
               new TollTimePrize(
                   new TimeSpan(0, 0, 0),
                   new TimeSpan(6, 0, 0),
                   (int)TollType.Free),
               new TollTimePrize(
                   new TimeSpan(6, 0, 0),
                   new TimeSpan(6, 30,0),
                   (int)TollType.LowTraffic),
               new TollTimePrize(
                  new TimeSpan(6, 30, 0),
                  new TimeSpan(7, 0,0),
                   (int)TollType.MidTraffic),
               new TollTimePrize(
                  new TimeSpan(7, 0, 0),
                  new TimeSpan(8, 0,0),
                   (int)TollType.RushHourTraffic),
               new TollTimePrize(
                  new TimeSpan(8, 0, 0),
                  new TimeSpan(8, 30,0),
                   (int)TollType.MidTraffic),
               new TollTimePrize(
                  new TimeSpan(8, 30, 0),
                  new TimeSpan(15, 00,0),
                   (int)TollType.LowTraffic),
               new TollTimePrize(
                  new TimeSpan(15, 0, 0),
                  new TimeSpan(15, 30,0),
                   (int)TollType.MidTraffic),
               new TollTimePrize(
                  new TimeSpan(15, 30, 0),
                  new TimeSpan(17, 00,0),
                   (int)TollType.RushHourTraffic),
               new TollTimePrize(
                  new TimeSpan(17, 0, 0),
                  new TimeSpan(18, 0,0),
                   (int)TollType.MidTraffic),
               new TollTimePrize(
                  new TimeSpan(18, 0, 0),
                  new TimeSpan(18, 30,0),
                   (int)TollType.LowTraffic),
               new TollTimePrize(
                  new TimeSpan(18, 30, 0),
                  new TimeSpan(24, 0, 0),
                   (int)TollType.Free)
            },
            tollFreeDates: GetTollFreeDates2013(),
            tollFreeVehicleTypes: new int[]
            {
                (int)VehicleType.Motorbike,
                (int)VehicleType.Tractor,
                (int)VehicleType.Emergency,
                (int)VehicleType.Diplomat,
                (int)VehicleType.Foreign,
                (int)VehicleType.Military
            });

        var trafficToll2022Json = JsonSerializer.Serialize(trafficToll2013, new JsonSerializerOptions {WriteIndented = true });
        _testOutputHelper.WriteLine(trafficToll2022Json);
    }

    private static DateTime[] GetTollFreeDates2013()
    {
        var singleDateHolidays = new[]
            {
                new DateTime(2023, 1, 1),
                new DateTime(2023, 3, 28),
                new DateTime(2023, 3, 29),
                new DateTime(2023, 4, 1),
                new DateTime(2023, 4, 20),
                new DateTime(2023, 5, 1),
                new DateTime(2023, 5, 8),
                new DateTime(2023, 5, 9),
                new DateTime(2023, 6, 5),
                new DateTime(2023, 6, 6),
                new DateTime(2023, 6, 21),
                new DateTime(2023, 11, 1),
                new DateTime(2023, 12, 24),
                new DateTime(2023, 12, 25),
                new DateTime(2023, 12, 26),
                new DateTime(2023, 12, 31)
        };

        var year = 2013;
        var month = 7;
        var datesOfJuly = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                         .Select(day => new DateTime(year, month, day))
                         .ToArray();

        var tollFreeDates2013 = new List<DateTime>();
        tollFreeDates2013.AddRange(singleDateHolidays);
        tollFreeDates2013.AddRange(datesOfJuly);
        return tollFreeDates2013.OrderBy(day => day).ToArray();
    }
}
