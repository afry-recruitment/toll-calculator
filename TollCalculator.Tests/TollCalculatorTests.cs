namespace TollFeeCalculator.Tests;

using System;
using TollFeeCalculator.Models;
using TollFeeCalculator.Services;
using Xunit;

public class TollCalculatorTests
{

    [Fact]
    public void GetTollFee_Car_SinglePassage()
    {
        var tollCalculator = new TollCalculator();
        var car = new Car();
        var dates = new DateTime[]
        {
            new DateTime(2025, 7, 28, 14, 30, 0)
        };

        int fee = tollCalculator.GetTotalTollFee(car, dates);

        Assert.Equal(8, fee);
    }

    [Fact]
    public void GetTollFee_Car_TwoPassesWithinOneHour_HighestFeeApplies()
    {
        var tollCalculator = new TollCalculator();
        var car = new Car();
        var dates = new DateTime[]
        {
            new DateTime(2025, 7, 28, 07, 30, 0), // 18 SEK
            new DateTime(2025, 7, 28, 08, 00, 0)  // 13 SEK (ska ignoreras)
        };

        int fee = tollCalculator.GetTotalTollFee(car, dates);

        Assert.Equal(18, fee);
    }

    [Fact]
    public void GetTollFee_Car_MultiplePassages_MaxDailyFeeApplied()
    {
        var tollCalculator = new TollCalculator();
        var car = new Car();
        var dates = new DateTime[]
        {
            new DateTime(2025, 7, 28, 06, 30, 0), // 13
            new DateTime(2025, 7, 28, 07, 31, 0), // 18
            new DateTime(2025, 7, 28, 08, 32, 0), // 13
            new DateTime(2025, 7, 28, 15, 33, 0), // 18
            new DateTime(2025, 7, 28, 17, 34, 0), // 13
            new DateTime(2025, 7, 28, 18, 35, 0)  // 8
        };

        int fee = tollCalculator.GetTotalTollFee(car, dates);

        Assert.Equal(60, fee); // Maxgräns ska tillämpas
    }

    [Fact]
    public void GetTollFee_Car_Weekend_NoFee()
    {
        var tollCalculator = new TollCalculator();
        var car = new Car();
        var dates = new DateTime[]
        {
            new DateTime(2025, 12, 24, 12, 0, 0) // Söndag
        };

        int fee = tollCalculator.GetTotalTollFee(car, dates);

        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_Motorbike_NoFee()
    {
        var tollCalculator = new TollCalculator();
        var motorbike = new Motorbike();
        var dates = new DateTime[]
        {
            new DateTime(2025, 7, 28, 08, 0, 0) // Rusningstid
        };

        int fee = tollCalculator.GetTotalTollFee(motorbike, dates);

        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_NullOrEmptyDates_ReturnsZero()
    {
        var tollCalculator = new TollCalculator();
        var car = new Car();

        Assert.Equal(0, tollCalculator.GetTotalTollFee(car, null));
        Assert.Equal(0, tollCalculator.GetTotalTollFee(car, Array.Empty<DateTime>()));
    }

    [Fact]
    public void GetTollFee_SameTimeMultipleEntries_OnlyHighestFeeCharged()
    {
        var tollCalculator = new TollCalculator();
        var car = new Car();
        var dates = new DateTime[]
        {
        new DateTime(2025, 7, 28, 08, 00, 00), // 13 SEK
        new DateTime(2025, 7, 28, 08, 00, 30), // 13 SEK - samma minut, ska inte adderas
        new DateTime(2025, 7, 28, 08, 59, 59), // fortfarande inom 60 min, max 13 SEK
        };

        int fee = tollCalculator.GetTotalTollFee(car, dates);
        Assert.Equal(13, fee);
    }

    [Fact]
    public void GetTollFee_Holiday_NoFee()
    {
        var tollCalculator = new TollCalculator();
        var car = new Car();
        // Anta att 1 januari är helgdag
        var dates = new DateTime[]
        {
        new DateTime(2025, 1, 1, 12, 0, 0)
        };

        int fee = tollCalculator.GetTotalTollFee(car, dates);
        Assert.Equal(0, fee);
    }

    [Fact]
    public void GetTollFee_MultipleDays_FeeIsCalculatedPerDay()
    {
        var tollCalculator = new TollCalculator();
        var car = new Car();
        var dates = new DateTime[]
        {
        new DateTime(2025, 7, 28, 07, 0, 0),  // Dag 1 - 18 SEK
        new DateTime(2025, 7, 28, 08, 0, 0),  // Dag 1 - 13 SEK (ignoreras inom timme)
        new DateTime(2025, 7, 29, 07, 0, 0),  // Dag 2 - 18 SEK
        new DateTime(2025, 7, 29, 18, 0, 0),  // Dag 2 - 8 SEK
        };

        int feeDay1 = tollCalculator.GetTotalTollFee(car, dates.Where(d => d.Date == new DateTime(2025, 7, 28)).ToArray());
        int feeDay2 = tollCalculator.GetTotalTollFee(car, dates.Where(d => d.Date == new DateTime(2025, 7, 29)).ToArray());

        Assert.Equal(18, feeDay1);
        Assert.Equal(26, feeDay2);
    }
}