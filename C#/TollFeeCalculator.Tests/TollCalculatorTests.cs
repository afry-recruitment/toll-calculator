using TollFeeCalculator.Policies;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests;

public class TollCalculatorTests
{
    // The tests use their own simple schedule and maximum, so they do not depend on the city's prices.
    private const int NoFee = 0;
    private const int LowFee = 10;
    private const int MediumFee = 20;
    private const int HighFee = 30;
    private const int MaxDailyFee = 100;

    private static readonly FeeSchedule Schedule = new(
    [
        (new TimeOnly(6, 0), LowFee),
        (new TimeOnly(7, 0), HighFee),
        (new TimeOnly(8, 0), MediumFee),
        (new TimeOnly(18, 0), NoFee),
    ]);

    private static readonly Vehicle Car = new(VehicleType.Car);
    private static readonly DateTime RushHour = At(7, 0);

    private readonly TollCalculator _calculator = new(Schedule, MaxDailyFee);

    [Fact]
    public void Car_in_rush_hour_pays_the_high_fee()
    {
        var fee = _calculator.GetTollFee(RushHour, Car);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Toll_free_vehicle_pays_nothing_in_rush_hour()
    {
        var fee = _calculator.GetTollFee(RushHour, new Vehicle(VehicleType.Motorbike));

        Assert.Equal(NoFee, fee);
    }

    [Fact]
    public void Car_pays_nothing_on_a_toll_free_date()
    {
        var saturdayRushHour = new DateTime(2013, 2, 2, 7, 0, 0);

        var fee = _calculator.GetTollFee(saturdayRushHour, Car);

        Assert.Equal(NoFee, fee);
    }

    [Fact]
    public void Missing_vehicle_is_charged_as_a_paying_vehicle()
    {
        var fee = _calculator.GetTollFee(RushHour, null!);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Unknown_vehicle_type_is_charged_as_a_paying_vehicle()
    {
        var fee = _calculator.GetTollFee(RushHour, new Vehicle((VehicleType)99));

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Single_passage_costs_its_fee()
    {
        var fee = _calculator.GetTollFee(Car, [At(7, 0)]);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Two_passages_within_an_hour_with_the_same_fee_are_charged_once()
    {
        var fee = _calculator.GetTollFee(Car, [At(6, 5), At(6, 25)]);

        Assert.Equal(LowFee, fee);
    }

    [Fact]
    public void Two_passages_within_an_hour_cost_the_higher_fee_when_the_second_is_higher()
    {
        var fee = _calculator.GetTollFee(Car, [At(6, 40), At(7, 10)]);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Two_passages_within_an_hour_cost_the_higher_fee_when_the_first_is_higher()
    {
        var fee = _calculator.GetTollFee(Car, [At(7, 50), At(8, 10)]);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Toll_free_vehicle_pays_nothing_for_a_day_of_passages()
    {
        var fee = _calculator.GetTollFee(new Vehicle(VehicleType.Motorbike), [At(7, 0), At(12, 0), At(16, 0)]);

        Assert.Equal(NoFee, fee);
    }

    [Fact]
    public void Passages_in_separate_hours_are_each_charged()
    {
        var fee = _calculator.GetTollFee(Car, [At(6, 0), At(15, 0)]);

        Assert.Equal(LowFee + MediumFee, fee);
    }

    [Fact]
    public void Three_passages_within_an_hour_cost_the_highest_fee_once()
    {
        var fee = _calculator.GetTollFee(Car, [At(6, 20), At(6, 40), At(7, 10)]);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void A_passage_after_the_hour_has_ended_starts_a_new_hour()
    {
        var fee = _calculator.GetTollFee(Car, [At(6, 0), At(6, 30), At(7, 10)]);

        Assert.Equal(LowFee + HighFee, fee);
    }

    [Fact]
    public void A_passage_exactly_60_minutes_later_belongs_to_the_same_hour()
    {
        var fee = _calculator.GetTollFee(Car, [At(6, 0), At(7, 0)]);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void A_passage_61_minutes_later_starts_a_new_hour()
    {
        var fee = _calculator.GetTollFee(Car, [At(6, 0), At(7, 1)]);

        Assert.Equal(LowFee + HighFee, fee);
    }

    [Fact]
    public void A_free_passage_does_not_start_the_hour()
    {
        var fee = _calculator.GetTollFee(Car, [At(5, 50), At(6, 30), At(7, 15)]);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void No_passages_cost_nothing()
    {
        var fee = _calculator.GetTollFee(Car, []);

        Assert.Equal(NoFee, fee);
    }

    [Fact]
    public void A_day_never_costs_more_than_the_daily_maximum()
    {
        var fee = _calculator.GetTollFee(Car, PassagesWorthMoreThanTheDailyMaximum);

        Assert.Equal(MaxDailyFee, fee);
    }

    [Fact]
    public void A_day_below_the_daily_maximum_is_charged_in_full()
    {
        var fee = _calculator.GetTollFee(Car, [At(6, 0), At(7, 5), At(8, 10), At(9, 15)]);

        Assert.Equal(LowFee + HighFee + MediumFee + MediumFee, fee);
    }

    [Fact]
    public void Passages_on_two_days_are_charged_for_each_day()
    {
        var nextDayRushHour = RushHour.AddDays(1);

        var fee = _calculator.GetTollFee(Car, [RushHour, nextDayRushHour]);

        Assert.Equal(HighFee + HighFee, fee);
    }

    [Fact]
    public void Each_day_has_its_own_daily_maximum()
    {
        DateTime[] monday = PassagesWorthMoreThanTheDailyMaximum;
        DateTime[] tuesday = [.. monday.Select(passage => passage.AddDays(1))];

        var fee = _calculator.GetTollFee(Car, [.. monday, .. tuesday]);

        Assert.Equal(MaxDailyFee + MaxDailyFee, fee);
    }

    [Fact]
    public void Passages_out_of_time_order_cost_the_same_as_in_time_order()
    {
        var inTimeOrder = _calculator.GetTollFee(Car, [At(6, 0), At(6, 30), At(7, 10)]);
        var outOfTimeOrder = _calculator.GetTollFee(Car, [At(7, 10), At(6, 0), At(6, 30)]);

        Assert.Equal(LowFee + HighFee, inTimeOrder);
        Assert.Equal(inTimeOrder, outOfTimeOrder);
    }

    [Fact]
    public void Missing_list_of_passages_is_rejected()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => _calculator.GetTollFee(Car, null!));

        Assert.Equal("dates", exception.ParamName);
    }

    [Fact]
    public void Calculator_without_arguments_uses_the_citys_schedule_and_daily_maximum()
    {
        var cityCalculator = new TollCalculator();

        // The city charges 18 SEK at 07:00 and at most 60 SEK per day.
        Assert.Equal(18, cityCalculator.GetTollFee(RushHour, Car));
        Assert.Equal(60, cityCalculator.GetTollFee(Car, PassagesWorthMoreThanTheDailyMaximum));
    }

    // Twelve passages more than an hour apart, worth more than the daily maximum
    // in both the test schedule (240) and the city's schedule (144).
    private static DateTime[] PassagesWorthMoreThanTheDailyMaximum =>
    [
        At(6, 0), At(7, 5), At(8, 10), At(9, 15), At(10, 20), At(11, 25),
        At(12, 30), At(13, 35), At(14, 40), At(15, 45), At(16, 50), At(17, 55),
    ];

    // A time on Monday 4 February 2013, an ordinary weekday.
    private static DateTime At(int hour, int minute)
    {
        return new DateTime(2013, 2, 4, hour, minute, 0);
    }
}