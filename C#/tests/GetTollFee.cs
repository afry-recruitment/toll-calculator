using TollFeeCalculator;

namespace TollFeeCalculator.Test;

public class UnitTest1
{
    /*
        Requirements:
        - The maximum fee for one day is 60 SEK.
        - A vehicle should only be charged once an hour. In case of multiple fees in the same hour period, the highest one applies.
        - Some vehicle types are fee-free.
        - Weekends and holidays are fee-free.
        - Every minute in span 00:00 - 05:59 should return 0 SEK.
        - Every minute in span 06:00 - 06:29 should return 8 SEK.
        - Every minute in span 06:30 - 06:59 should return 13 SEK.
        - Every minute in span 07:00 - 07:59 should return 18 SEK.
        - Every minute in span 08:00 - 08:29 should return 13 SEK.
        - Every minute in span 08:30 - 14:59 should return 8 SEK.
        - Every minute in span 15:00 - 15:29 should return 13 SEK.
        - Every minute in span 15:30 - 16:59 should return 18 SEK.
        - Every minute in span 17:00 - 17:59 should return 13 SEK.
        - Every minute in span 18:00 - 18:29 should return 8 SEK.
        - Every minute in span 18:30 - 23:59 should return 0 SEK.
    */

    // TEST CASE: The maximum fee for one day is 60 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void MaximumFeeOnNonFeeFreeDayIsSixty(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        
        var dates = new DateTime[6];
        dates[0] = new DateTime(2013, 1, 1, 6, 30, 0, 0).AddDays(daysToAdd);
        dates[1] = new DateTime(2013, 1, 1, 7, 30, 0, 0).AddDays(daysToAdd);
        dates[2] = new DateTime(2013, 1, 1, 8, 30, 0, 0).AddDays(daysToAdd);
        dates[3] = new DateTime(2013, 1, 1, 9, 30, 0, 0).AddDays(daysToAdd);
        dates[4] = new DateTime(2013, 1, 1, 10, 30, 0, 0).AddDays(daysToAdd);
        dates[5] = new DateTime(2013, 1, 1, 11, 30, 0, 0).AddDays(daysToAdd);

        var result = tollCalculator.GetTollFee(car, dates);
        Assert.True(result == 60);
    }

    // TEST CASE: A vehicle should only be charged once an hour. In case of multiple fees in the same hour period, the highest one applies.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void VehicleIsOnlyChargedHighestFeeIfMultipleFeesWithinOneHour(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
    
        var dates = new DateTime[4];
        dates[0] = new DateTime(2013, 1, 1, 6, 35, 0, 0).AddDays(daysToAdd);
        dates[1] = new DateTime(2013, 1, 1, 6, 45, 0, 0).AddDays(daysToAdd);
        dates[2] = new DateTime(2013, 1, 1, 6, 50, 0, 0).AddDays(daysToAdd);
        dates[3] = new DateTime(2013, 1, 1, 7, 30, 0, 0).AddDays(daysToAdd);

        var result = tollCalculator.GetTollFee(car, dates);
        Assert.True(result == 18);
    }

    // TEST CASE: Weekends and holidays are fee-free.
    [Theory]
    [ClassData(typeof(FeeFreeDays))]
    public void WeekEndsAndHolidaysAreFeeFree(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);

        const int minutesInDay = 24 * 60;
        for (int i = 0; i < minutesInDay; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 0);
        }
    }

    // TEST CASE: Some vehicle types are fee-free. In our case only cars are charged.
    [Theory]
    [ClassData(typeof(AllDays))]
    public void NoFeesChargedIfVehicleIsTollFree(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var vehicles = new List<Vehicle>()
        {
            new Diplomat(),
            new Emergency(),
            new Foreign(),
            new Military(),
            new Motorbike(),
            new Tractor()
        };
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);

        foreach (Vehicle vehicle in vehicles)
        {
            const int minutesInDay = 24 * 60;
            for (int i = 0; i < minutesInDay; i++)
            {
                var dates = new DateTime[1];
                dates[0] = date.AddMinutes(i);
                var result = tollCalculator.GetTollFee(vehicle, dates);
                Assert.True(result == 0);
            }
        }
    }

    // TEST CASE: Every minute in span 00:00 - 05:59 should return 0 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_0000_0559_ReturnsZero(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);

        const int minutes = 6 * 60;
        for (int i = 0; i < minutes; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 0);
        }
    }

    // TEST CASE: Every minute in span 06:00 - 06:29 should return 8 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_0600_0629_ReturnsEight(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);
        
        const int t_0600 = 6 * 60;
        const int limit = t_0600 + 30;
        for (int i = t_0600; i < limit; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 8);
        }
    }

    // TEST CASE: Every minute in span 06:30 - 06:59 should return 13 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_0630_0659_ReturnsThirteen(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);
        
        const int t_0630 = (6 * 60) + 30;
        const int limit = t_0630 + 30;
        for (int i = t_0630; i < limit; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 13);
        }
    }

    // TEST CASE: Every minute in span 07:00 - 07:59 should return 18 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_0700_0759_ReturnsEighteen(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);
        
        const int t_0700 = 7 * 60;
        const int limit = t_0700 + 60;
        for (int i = t_0700; i < limit; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 18);
        }
    }

    // TEST CASE: Every minute in span 08:00 - 08:29 should return 13 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_0800_0829_ReturnsThirteen(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);
        
        const int t_0800 = 8 * 60;
        const int limit = t_0800 + 30;
        for (int i = t_0800; i < limit; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 13);
        }
    }

    // TEST CASE: Every minute in span 08:30 - 14:59 should return 8 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_0830_1459_ReturnsEight(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);
        
        const int t_0830 = (8 * 60) + 30;
        const int t_1459 = (14 * 60) + 59;
        const int limit = t_1459 + 1;
        for (int i = t_0830; i < limit; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 8);
        }
    }

    // TEST CASE: Every minute in span 15:00 - 15:29 should return 13 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_1500_1529_ReturnsThirteen(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);
        
        const int t_1500 = 15 * 60;
        const int limit = t_1500 + 30;
        for (int i = t_1500; i < limit; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 13);
        }
    }

    // TEST CASE: Every minute in span 15:30 - 16:59 should return 18 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_1530_1659_ReturnsEighteen(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);
        
        const int t_1530 = (15 * 60) + 30;
        const int limit = t_1530 + 90;
        for (int i = t_1530; i < limit; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 18);
        }
    }

    // TEST CASE: Every minute in span 17:00 - 17:59 should return 13 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_1700_1759_ReturnsThirteen(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);
        
        const int t_1700 = 17 * 60;
        const int limit = t_1700 + 60;
        for (int i = t_1700; i < limit; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 13);
        }
    }

    // TEST CASE: Every minute in span 18:00 - 18:29 should return 8 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_1800_1829_ReturnsEight(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);
        
        const int t_1800 = 18 * 60;
        const int limit = t_1800 + 30;
        for (int i = t_1800; i < limit; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 8);
        }
    }

    // TEST CASE: Every minute in span 18:30 - 23:59 should return 0 SEK.
    [Theory]
    [ClassData(typeof(NonFeeFreeDays))]
    public void t_1830_2359_ReturnsZero(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);
        
        const int t_1830 = (18 * 60) + 30;
        const int t_2359 = (23 * 60) + 59;
        const int limit = t_2359 + 1;
        for (int i = t_1830; i < limit; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            var result = tollCalculator.GetTollFee(car, dates);
            Assert.True(result == 0);
        }
    }

    [Theory]
    [ClassData(typeof(AllDays))]
    public void IfYearIsNot2013ThrowsArgumentException(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var car = new Car();
        var date = new DateTime(2014, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);

        const int minutesInDay = 24 * 60;
        for (int i = 0; i < minutesInDay; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            Assert.Throws<ArgumentException>(() => tollCalculator.GetTollFee(car, dates));
        }
    }

    [Theory]
    [ClassData(typeof(AllDays))]
    public void IfVehicleIsNullThrowsArgumentNullException(int dayOfYear)
    {
        int daysToAdd = dayOfYear - 1;

        var tollCalculator = new TollCalculator();
        var date = new DateTime(2013, 1, 1, 0, 0, 0, 0).AddDays(daysToAdd);

        const int minutesInDay = 24 * 60;
        for (int i = 0; i < minutesInDay; i++)
        {
            var dates = new DateTime[1];
            dates[0] = date.AddMinutes(i);
            Assert.Throws<ArgumentNullException>(() => tollCalculator.GetTollFee(null, dates));
        }
    }

    [Fact]
    public void IfDatesIsNullThrowsArgumentNullException()
    {
        var tollCalculator = new TollCalculator();
        var car = new Car();
        Assert.Throws<ArgumentNullException>(() => tollCalculator.GetTollFee(car, null));
    }
}
