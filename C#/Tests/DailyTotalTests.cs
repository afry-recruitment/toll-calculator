namespace TollFeeCalculator.Tests;

/// <summary>
/// Rules: "The maximum fee for one day is 60 SEK" and "A vehicle should only be
/// charged once an hour. In the case of multiple fees in the same hour period,
/// the highest one applies."
/// </summary>
public class DailyTotalTests
{
    private readonly TollCalculator _calculator = new();

    private int Fee(params DateTime[] passes) => _calculator.GetTollFee(new Car(), passes);

    [Fact]
    public void Two_passes_within_one_hour_charge_only_the_highest()
    {
        // 06:15 (8) and 06:45 (13) fall in the same hour -> 13.
        Assert.Equal(13, Fee(Days.Tue(6, 15), Days.Tue(6, 45)));
    }

    [Fact]
    public void Three_passes_within_one_hour_charge_only_the_highest()
    {
        // Regression: used to overcharge (18) by subtracting the anchor fee.
        Assert.Equal(13, Fee(Days.Tue(6, 15), Days.Tue(6, 45), Days.Tue(6, 50)));
    }

    [Fact]
    public void Four_passes_within_one_hour_charge_only_the_highest()
    {
        Assert.Equal(13, Fee(Days.Tue(6, 10), Days.Tue(6, 20), Days.Tue(6, 40), Days.Tue(6, 55)));
    }

    [Fact]
    public void Passes_in_different_hours_add_up()
    {
        // 06:15 (8) and 08:00 (13) are more than an hour apart -> 21.
        Assert.Equal(21, Fee(Days.Tue(6, 15), Days.Tue(8, 0)));
    }

    [Fact]
    public void Exactly_sixty_minutes_apart_is_one_window()
    {
        // 07:00 (18) and 08:00 (13) are exactly 60 minutes apart -> same window, max 18.
        Assert.Equal(18, Fee(Days.Tue(7, 0), Days.Tue(8, 0)));
    }

    [Fact]
    public void Just_over_sixty_minutes_apart_is_two_windows()
    {
        // 07:00 (18) and 08:01 (13) are 61 minutes apart -> 31.
        Assert.Equal(31, Fee(Days.Tue(7, 0), Days.Tue(8, 1)));
    }

    [Fact]
    public void Daily_total_is_capped_at_60()
    {
        int total = Fee(
            Days.Tue(6, 45),
            Days.Tue(7, 30),
            Days.Tue(8, 10),
            Days.Tue(15, 10),
            Days.Tue(16, 30),
            Days.Tue(17, 30));

        Assert.Equal(60, total);
    }

    [Fact]
    public void Very_expensive_day_never_exceeds_the_cap()
    {
        // A pass in every expensive hour of the day.
        int total = Fee(
            Days.Tue(6, 45), Days.Tue(7, 30), Days.Tue(8, 10),
            Days.Tue(15, 10), Days.Tue(16, 30), Days.Tue(17, 30),
            Days.Tue(18, 0));

        Assert.Equal(60, total);
    }
}
