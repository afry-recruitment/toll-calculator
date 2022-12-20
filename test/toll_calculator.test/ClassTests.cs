//using FluentAssertions;
//using toll_calculator.valueobjects;
//using Xunit;

//namespace toll_calculator.tests;


//public class ClassTests
//{
//    //Fees will differ between 8 SEK and 18 SEK, depending on the time of day

//    [Fact]
//    public void GetTollFee_car_one_passing_all_ranges_happy_path()
//    {
//        TestCarToll(5, 59).Should().Be(0);
//        TestCarToll(6, 0).Should().Be(TollFee.LowTraffic);
//        TestCarToll(6, 29, 59).Should().Be(TollFee.LowTraffic);
//        TestCarToll(6, 30).Should().Be(TollFee.MidTraffic);
//        TestCarToll(6, 59).Should().Be(TollFee.MidTraffic);
//        TestCarToll(7, 0).Should().Be(TollFee.RushHourTraffic);
//        TestCarToll(7, 59).Should().Be(TollFee.RushHourTraffic);
//        TestCarToll(8, 0).Should().Be(TollFee.MidTraffic);
//        TestCarToll(8, 29, 59).Should().Be(TollFee.MidTraffic);
//        TestCarToll(8, 30).Should().Be(TollFee.LowTraffic);
//        TestCarToll(14, 59).Should().Be(TollFee.LowTraffic);
//        TestCarToll(15, 00).Should().Be(TollFee.MidTraffic);
//        TestCarToll(14, 59).Should().Be(TollFee.LowTraffic);
//        TestCarToll(15, 0).Should().Be(TollFee.MidTraffic);
//        TestCarToll(15, 29, 59).Should().Be(TollFee.MidTraffic);
//        TestCarToll(16, 0).Should().Be(TollFee.RushHourTraffic);
//        TestCarToll(16, 59).Should().Be(TollFee.RushHourTraffic);
//        TestCarToll(17, 00).Should().Be(TollFee.MidTraffic);
//        TestCarToll(17, 59).Should().Be(TollFee.MidTraffic);
//        TestCarToll(18, 00).Should().Be(TollFee.LowTraffic);
//        TestCarToll(18, 29, 59).Should().Be(TollFee.LowTraffic);
//        TestCarToll(18, 30).Should().Be(0);
//        TestCarToll(23, 59).Should().Be(0);
//        TestCarToll(0, 0, 0).Should().Be(0);
//    }

//    private int TestCarToll(int hours, int minutes, int seconds = 0, int ms = 0)
//    {
//        var passing = new DateTime(2022, 12, 19, hours, minutes, seconds, ms, DateTimeKind.Local);
//        return TollTimeOfDayCalculator.GetTollFee(passing);
//    }
//}
