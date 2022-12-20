using FluentAssertions;
using toll_calculator.enums;
using toll_calculator.models;
using toll_calculator.repository;
using Xunit;

namespace toll_calculator.tests;


public class TrafficTollRepositoryTests
{
    //Fees will differ between 8 SEK and 18 SEK, depending on the time of day

    [Fact]
    public void Get_traffic_toll_from_repository_assert_no_exception()
    {
        var trafficTollSpec = TrafficTollSpecificationRepository.GetTrafficTollSpecification();
        trafficTollSpec.ValidFrom.Year.Should().Be(2022);
    }

        [Fact]
    public void Create_traffic_toll_for_2022()
    {
        var trafficToll2022 = new TrafficTollSpecification(
            validFrom: new DateTime(2022, 1, 1),
            validUntil: new DateTime(2022, 12, 31),
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
            tollFreeDates: new DateTime[0],
            tollFreeVehicleTypes: new int[]
            {
                (int)VehicleType.Motorbike,
                (int)VehicleType.Tractor,
                (int)VehicleType.Emergency,
                (int)VehicleType.Diplomat,
                (int)VehicleType.Foreign,
                (int)VehicleType.Military
            });

        //File.WriteAllText("traffic_toll_2022.json", JsonSerializer.Serialize(trafficToll2022));
    }
}
