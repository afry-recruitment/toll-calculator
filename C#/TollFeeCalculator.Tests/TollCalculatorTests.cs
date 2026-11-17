using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests;

public class TollCalculatorTests
{
    private const int NoFee = 0;
    private const int LowFee = 8;
    private const int MediumFee = 13;
    private const int HighFee = 18;
    private const int MaxDailyFee = 60;

    private static readonly Vehicle Car = new(VehicleType.Car);
    private static readonly DateTime RushHour = At(7, 0);

    [Fact]
    public void Car_in_morning_rush_hour_pays_18()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(RushHour, Car);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Toll_free_vehicle_pays_nothing_in_rush_hour()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(RushHour, new Vehicle(VehicleType.Motorbike));

        Assert.Equal(NoFee, fee);
    }

    [Fact]
    public void Car_pays_nothing_on_a_toll_free_date()
    {
        var calculator = new TollCalculator();
        var saturdayRushHour = new DateTime(2013, 2, 2, 7, 0, 0);

        var fee = calculator.GetTollFee(saturdayRushHour, Car);

        Assert.Equal(NoFee, fee);
    }

    [Fact]
    public void Missing_vehicle_is_charged_as_a_paying_vehicle()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(RushHour, null!);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Unknown_vehicle_type_is_charged_as_a_paying_vehicle()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(RushHour, new Vehicle((VehicleType)99));

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Single_passage_costs_its_fee()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(7, 0)]);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Two_passages_within_an_hour_with_the_same_fee_are_charged_once()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(6, 5), At(6, 25)]);

        Assert.Equal(LowFee, fee);
    }

    [Fact]
    public void Two_passages_within_an_hour_cost_the_higher_fee_when_the_second_is_higher()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(6, 20), At(6, 40)]);

        Assert.Equal(MediumFee, fee);
    }

    [Fact]
    public void Two_passages_within_an_hour_cost_the_higher_fee_when_the_first_is_higher()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(7, 50), At(8, 10)]);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void Toll_free_vehicle_pays_nothing_for_a_day_of_passages()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(new Vehicle(VehicleType.Motorbike), [At(7, 0), At(12, 0), At(16, 0)]);

        Assert.Equal(NoFee, fee);
    }

    [Fact]
    public void Passages_in_separate_hours_are_each_charged()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(6, 0), At(15, 0)]);

        Assert.Equal(LowFee + MediumFee, fee);
    }

    [Fact]
    public void Three_passages_within_an_hour_cost_the_highest_fee_once()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(6, 20), At(6, 40), At(7, 10)]);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void A_passage_after_the_hour_has_ended_starts_a_new_hour()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(6, 0), At(6, 30), At(7, 10)]);

        Assert.Equal(MediumFee + HighFee, fee);
    }

    [Fact]
    public void A_passage_exactly_60_minutes_later_belongs_to_the_same_hour()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(6, 0), At(7, 0)]);

        Assert.Equal(HighFee, fee);
    }

    [Fact]
    public void A_passage_61_minutes_later_starts_a_new_hour()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(6, 0), At(7, 1)]);

        Assert.Equal(LowFee + HighFee, fee);
    }

    [Fact]
    public void A_free_passage_does_not_start_the_hour()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(5, 50), At(6, 10), At(6, 55)]);

        Assert.Equal(MediumFee, fee);
    }

    [Fact]
    public void No_passages_cost_nothing()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, []);

        Assert.Equal(NoFee, fee);
    }

    [Fact]
    public void A_day_never_costs_more_than_the_daily_maximum()
    {
        var calculator = new TollCalculator();

        // Twelve passages more than an hour apart, worth 144 SEK without the maximum.
        DateTime[] passages =
        [
            At(6, 0), At(7, 5), At(8, 10), At(9, 15), At(10, 20), At(11, 25),
            At(12, 30), At(13, 35), At(14, 40), At(15, 45), At(16, 50), At(17, 55),
        ];

        var fee = calculator.GetTollFee(Car, passages);

        Assert.Equal(MaxDailyFee, fee);
    }

    [Fact]
    public void A_day_below_the_daily_maximum_is_charged_in_full()
    {
        var calculator = new TollCalculator();

        var fee = calculator.GetTollFee(Car, [At(7, 0), At(8, 5), At(9, 10), At(15, 30)]);

        Assert.Equal(HighFee + MediumFee + LowFee + HighFee, fee);
    }

    [Fact]
    public void Passages_on_two_days_are_charged_for_each_day()
    {
        var calculator = new TollCalculator();
        var nextDayRushHour = RushHour.AddDays(1);

        var fee = calculator.GetTollFee(Car, [RushHour, nextDayRushHour]);

        Assert.Equal(HighFee + HighFee, fee);
    }

    [Fact]
    public void Each_day_has_its_own_daily_maximum()
    {
        var calculator = new TollCalculator();

        // Twelve passages worth 144 SEK on Monday, and the same again on Tuesday.
        DateTime[] monday =
        [
            At(6, 0), At(7, 5), At(8, 10), At(9, 15), At(10, 20), At(11, 25),
            At(12, 30), At(13, 35), At(14, 40), At(15, 45), At(16, 50), At(17, 55),
        ];
        DateTime[] tuesday = [.. monday.Select(passage => passage.AddDays(1))];

        var fee = calculator.GetTollFee(Car, [.. monday, .. tuesday]);

        Assert.Equal(MaxDailyFee + MaxDailyFee, fee);
    }

    [Fact]
    public void Passages_out_of_time_order_cost_the_same_as_in_time_order()
    {
        var calculator = new TollCalculator();

        var inTimeOrder = calculator.GetTollFee(Car, [At(6, 0), At(6, 30), At(7, 10)]);
        var outOfTimeOrder = calculator.GetTollFee(Car, [At(7, 10), At(6, 0), At(6, 30)]);

        Assert.Equal(MediumFee + HighFee, inTimeOrder);
        Assert.Equal(inTimeOrder, outOfTimeOrder);
    }

    [Fact]
    public void Missing_list_of_passages_is_rejected()
    {
        var calculator = new TollCalculator();

        var exception = Assert.Throws<ArgumentNullException>(() => calculator.GetTollFee(Car, null!));

        Assert.Equal("dates", exception.ParamName);
    }

    // A time on Monday 4 February 2013, an ordinary weekday.
    private static DateTime At(int hour, int minute)
    {
        return new DateTime(2013, 2, 4, hour, minute, 0);
    }
}