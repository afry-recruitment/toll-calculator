using System;
using TollFeeCalculator;
using Xunit;


public class TollCalculatorDailyFeeTests
{
    [Theory]
    [InlineData(6, 0, 8)]      // 06:00 -> 8 kr
    [InlineData(6, 29, 8)]     // gränsvärde: sista minuten i 8kr-bandet

    public void GetTollFee_Vehicle_passes_once_ReturnsExpectedFee(int hour, int minute, int expectedFee)
    {
        var calculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 2, 7, hour, minute, 0); // a weekday (Thursday)
        DateTime[] passages = new DateTime[] { date };

        int actualFee = calculator.GetTollFee(car, passages);

        Assert.Equal(expectedFee, actualFee);
    }

    [Theory]
    [InlineData(15, 15, 15, 45, 18)]      // Two passages within an hour with different fees, return highest fee -> 18 kr
    [InlineData(06, 15, 17, 30, 21)]      // Two passages during the day an hour with different fees, return sum of both fees -> 21 kr
    [InlineData(06, 30, 07, 29, 18)]      // Two passages during the day an hour with different fees, (59 minutes span between passages), return fee -> 18 kr
    [InlineData(06, 30, 07, 30, 18)]      // Two passages during the day an hour with different fees, (60 minutes span between passages), fee -> 18 kr
    [InlineData(06, 30, 07, 31, 31)]      // Two passages during the day an hour with different fees, (61 minutes span between passages), return sum of both fees -> 31 kr





    public void GetTollFee_Vehicle_passes_twice_ReturnsExpectedFee(int hour1, int minute1, int hour2, int minute2, int expectedFee)
    {
        var calculator = new TollCalculator();
        var car = new Car();
        var date1 = new DateTime(2013, 2, 7, hour1, minute1, 0); // a weekday (Thursday)
        var date2 = new DateTime(2013, 2, 7, hour2, minute2, 0); // a weekday (Thursday)

        DateTime[] passages = new DateTime[] { date1, date2 };


        int actualFee = calculator.GetTollFee(car, passages);

        Assert.Equal(expectedFee, actualFee);
    }
    [Theory]
    [InlineData(06, 45, 15, 15, 16, 00, 31)]      // Three passages, two within an hour with different fees (pick highest), return total fee -> 31 kr

    public void GetTollFee_Vehicle_passes_three_times_twice_within_same_hour_ReturnsExpectedFee(int hour1, int minute1, int hour2, int minute2, int hour3, int minute3, int expectedFee)
    {
        var calculator = new TollCalculator();
        var car = new Car();
        var date1 = new DateTime(2013, 2, 7, hour1, minute1, 0); // a weekday (Thursday)
        var date2 = new DateTime(2013, 2, 7, hour2, minute2, 0); // a weekday (Thursday)
        var date3 = new DateTime(2013, 2, 7, hour3, minute3, 0); // a weekday (Thursday)

        DateTime[] passages = new DateTime[] { date1, date2, date3 };

        int actualFee = calculator.GetTollFee(car, passages);

        Assert.Equal(expectedFee, actualFee);
    }

    [Fact]
    public void GetTollFee_Vehicle_passes_five_times_spread_out_ReturnsExpectedFee()
    {
        var calculator = new TollCalculator();
        var car = new Car();

        // Five passages spread across the day, covering three separate 60-minute windows
        DateTime[] passages = new DateTime[] 
        {
            new DateTime(2013, 2, 7, 7, 0, 0),  // = 18
            new DateTime(2013, 2, 7, 10, 0, 0), // = 8
            new DateTime(2013, 2, 7, 15, 0, 0), // = 13  first in hour slot
            new DateTime(2013, 2, 7, 15, 30, 0),// = 18  second in hour slot
            new DateTime(2013, 2, 7, 16, 0, 0)  //= 18   third in hour slot (only the highest of the three counts (18))
        };

        int expectedFee = 44; // expectedFee = 18+8+18 = 44

        int actualFee = calculator.GetTollFee(car, passages);

        Assert.Equal(expectedFee, actualFee);
    }

    [Fact]
    public void GetTollFee_Vehicle_passes_four_times_combined_fee_exceeds_daily_limit_60_kr_ReturnsExpectedFee()
    {
        var calculator = new TollCalculator();
        var car = new Car();

        // Four passages spread across the day, individual fees added (67 kr) exceeds limit (60 kr) which is the fee rturned.
        DateTime[] passages = new DateTime[]
        {
            new DateTime(2013, 2, 7, 6, 45, 0),  // = 13
            new DateTime(2013, 2, 7, 7, 55, 0),  // = 18
            new DateTime(2013, 2, 7, 15, 30, 0), // = 18  
            new DateTime(2013, 2, 7, 16, 35, 0) // = 18
        };

        int expectedFee = 60; // Daily limit fee

        int actualFee = calculator.GetTollFee(car, passages);

        Assert.Equal(expectedFee, actualFee);
    }
}
