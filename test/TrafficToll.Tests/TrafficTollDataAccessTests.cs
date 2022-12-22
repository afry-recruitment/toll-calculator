using FluentAssertions;
using System.Text.Json;
using TrafficToll.Internals.Enums;
using Xunit;
using Xunit.Abstractions;
using TrafficToll.Internals.DataAccess;
using TrafficToll.Internals.DataAccess.Models;

namespace TrafficToll.tests;


public class TrafficTollDataAccessTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TrafficTollDataAccessTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Serialize_traffic_toll_to_json()
    {
        //Arrange
        var trafficToll2013 = CreateTrafficTollSpecification();

        //Act & Assert
        var trafficToll2022Json = JsonSerializer.Serialize(trafficToll2013, new JsonSerializerOptions { WriteIndented = true });
        
        _testOutputHelper.WriteLine(trafficToll2022Json);
    }

    private static TrafficTollSpecification CreateTrafficTollSpecification()
    {
        return new TrafficTollSpecification(
            validFrom: new DateTime(2013, 1, 1),
            validUntil: new DateTime(2013, 12, 31),
            maximumDailyFee: 60,
            priceMapping: new Dictionary<int, int>()
            {
                { 0, 0 },
                { 1, 8 },
                { 2, 12 },
                { 3, 18 }
            },
            validTollTime: new TimeSpan(1, 0, 0),
            dailyTollTimePrizes: new[]
            {
               new TollTimePeriod(
                   new TimeSpan(0, 0, 0),
                   new TimeSpan(6, 0, 0),
                   (int)TollTrafficType.Free),
               new TollTimePeriod(
                   new TimeSpan(6, 0, 0),
                   new TimeSpan(6, 30,0),
                   (int)TollTrafficType.LowTraffic),
               new TollTimePeriod(
                  new TimeSpan(6, 30, 0),
                  new TimeSpan(7, 0,0),
                   (int)TollTrafficType.MidTraffic),
               new TollTimePeriod(
                  new TimeSpan(7, 0, 0),
                  new TimeSpan(8, 0,0),
                   (int)TollTrafficType.RushHourTraffic),
               new TollTimePeriod(
                  new TimeSpan(8, 0, 0),
                  new TimeSpan(8, 30,0),
                   (int)TollTrafficType.MidTraffic),
               new TollTimePeriod(
                  new TimeSpan(8, 30, 0),
                  new TimeSpan(15, 00,0),
                   (int)TollTrafficType.LowTraffic),
               new TollTimePeriod(
                  new TimeSpan(15, 0, 0),
                  new TimeSpan(15, 30,0),
                   (int)TollTrafficType.MidTraffic),
               new TollTimePeriod(
                  new TimeSpan(15, 30, 0),
                  new TimeSpan(17, 00,0),
                   (int)TollTrafficType.RushHourTraffic),
               new TollTimePeriod(
                  new TimeSpan(17, 0, 0),
                  new TimeSpan(18, 0,0),
                   (int)TollTrafficType.MidTraffic),
               new TollTimePeriod(
                  new TimeSpan(18, 0, 0),
                  new TimeSpan(18, 30,0),
                   (int)TollTrafficType.LowTraffic),
               new TollTimePeriod(
                  new TimeSpan(18, 30, 0),
                  new TimeSpan(24, 0, 0),
                   (int)TollTrafficType.Free)
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
    }

    private static DateTime[] GetTollFreeDates2013()
    {
        var singleDateHolidays = new[]
            {
                new DateTime(2013, 1, 1),
                new DateTime(2013, 3, 28),
                new DateTime(2013, 3, 29),
                new DateTime(2013, 4, 1),
                new DateTime(2013, 4, 20),
                new DateTime(2013, 5, 1),
                new DateTime(2013, 5, 8),
                new DateTime(2013, 5, 9),
                new DateTime(2013, 6, 5),
                new DateTime(2013, 6, 6),
                new DateTime(2013, 6, 21),
                new DateTime(2013, 11, 1),
                new DateTime(2013, 12, 24),
                new DateTime(2013, 12, 25),
                new DateTime(2013, 12, 26),
                new DateTime(2013, 12, 31)
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
