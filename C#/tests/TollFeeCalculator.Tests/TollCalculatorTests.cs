using TollFeeCalculator;
using Xunit;

namespace TollFeeCalculator.Tests;

public class TollCalculatorTests
{
    private readonly TollCalculator calculator = new();
    private const VehicleType car = VehicleType.Car;

    // A normal, non-exempt weekday in the supplied calendar.
    private static DateTime At(int hour, int minute = 0) => new(2013, 2, 4, hour, minute, 0);

    [Fact]
    public void Empty_input_is_free() => Assert.Equal(0, calculator.CalculateDailyFee(car, Array.Empty<DateTime>()));

    [Fact]
    public void Single_passage_is_charged_once() => Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(7) }));

    [Fact]
    public void Highest_fee_in_window_is_charged_once()
        => Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(6), At(6, 30), At(7) }));

    [Fact]
    public void Later_lower_fees_do_not_reduce_the_window_maximum()
        => Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(7, 30), At(8), At(8, 15), At(8, 30) }));

    [Fact]
    public void Repeated_increases_do_not_double_count()
        => Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(6), At(6, 30), At(6, 45), At(7) }));

    [Fact]
    public void Exactly_sixty_minutes_is_included()
        => Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(6), At(7) }));

    [Fact]
    public void One_tick_after_sixty_minutes_starts_a_new_window()
        => Assert.Equal(26, calculator.CalculateDailyFee(car, new[] { At(6), At(7).AddTicks(1) }));

    [Fact]
    public void Window_is_anchored_to_first_passage_not_last_or_highest()
        => Assert.Equal(31, calculator.CalculateDailyFee(car, new[] { At(6), At(6, 50), At(7, 40) }));

    [Fact]
    public void Each_new_window_gets_a_new_start()
        => Assert.Equal(39, calculator.CalculateDailyFee(car, new[] { At(6), At(7, 1), At(7, 45), At(8, 2), At(8, 30) }));

    [Fact]
    public void Hours_are_elapsed_time_not_millisecond_components()
        => Assert.Equal(26, calculator.CalculateDailyFee(car, new[] { At(7), At(12) }));

    [Fact]
    public void Zero_fee_passage_does_not_start_a_window()
        => Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(5, 59), At(6), At(7) }));

    [Fact]
    public void Adding_free_passages_does_not_change_the_daily_total()
    {
        var random = new Random(2026);
        for (int sample = 0; sample < 200; sample++)
        {
            DateTime[] chargedPassages = Enumerable.Range(0, random.Next(1, 20))
                .Select(_ => At(6).AddSeconds(random.Next(750 * 60))).ToArray();
            DateTime[] withFreePassages = chargedPassages.Concat(new[]
            {
                At(0), At(5, 59), At(6).AddTicks(-1), At(18, 30), At(23, 59)
            }).Reverse().ToArray();

            Assert.Equal(calculator.CalculateDailyFee(car, chargedPassages),
                calculator.CalculateDailyFee(car, withFreePassages));
        }
    }

    [Fact]
    public void Free_evening_passages_do_not_change_an_existing_charge()
        => Assert.Equal(8, calculator.CalculateDailyFee(car,
            new[] { At(18, 20), At(18, 30), At(19, 21), At(23, 59) }));

    [Fact]
    public void All_night_passages_are_free()
        => Assert.Equal(0, calculator.CalculateDailyFee(car, new[] { At(0), At(5, 59), At(18, 30), At(23, 59) }));

    [Fact]
    public void Input_can_be_unordered_and_is_not_mutated()
    {
        var dates = new[] { At(15, 30), At(6, 30), At(7), At(6) };
        var original = dates.ToArray();
        Assert.Equal(36, calculator.CalculateDailyFee(car, dates));
        Assert.Equal(original, dates);
    }

    [Fact]
    public void Duplicate_passages_are_not_double_charged()
        => Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(7), At(7), At(7) }));

    [Fact]
    public void Daily_total_is_capped_at_sixty()
    {
        var dates = new[] { At(6), At(7, 1), At(8, 2), At(9, 3), At(10, 4), At(15, 30), At(16, 31) };
        Assert.Equal(60, calculator.CalculateDailyFee(car, dates));
    }

    [Fact]
    public void Daily_total_exactly_at_the_cap_is_sixty()
        => Assert.Equal(60, calculator.CalculateDailyFee(car,
            new[] { At(7), At(9), At(11), At(13), At(15, 30) }));

    [Fact]
    public void Separate_days_do_not_share_state()
    {
        Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(7) }));
        Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(7).AddDays(1) }));
        Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(7) }));
    }

    [Theory]
    [InlineData(VehicleType.Motorbike)]
    [InlineData(VehicleType.Tractor)]
    [InlineData(VehicleType.Emergency)]
    [InlineData(VehicleType.Diplomat)]
    [InlineData(VehicleType.Foreign)]
    [InlineData(VehicleType.Military)]
    public void Listed_vehicle_types_are_exempt(VehicleType vehicle)
    {
        Assert.Equal(0, calculator.GetPassageFee(vehicle, At(7)));
        Assert.Equal(0, calculator.CalculateDailyFee(vehicle, new[] { At(7), At(15, 30) }));
    }

    [Theory]
    [InlineData(VehicleType.Car)]
    [InlineData(VehicleType.Other)]
    public void Nonexempt_categories_are_chargeable(VehicleType vehicle)
        => Assert.Equal(18, calculator.GetPassageFee(vehicle, At(7)));

    [Theory]
    [InlineData(2, 2)]
    [InlineData(2, 3)]
    [InlineData(7, 1)]
    [InlineData(7, 31)]
    public void Weekends_and_july_are_free(int month, int day)
        => Assert.Equal(0, calculator.CalculateDailyFee(car, new[] { new DateTime(2013, month, day, 7, 0, 0) }));

    [Fact]
    public void Mixed_days_are_rejected_even_if_daily_cap_would_have_been_reached()
    {
        var dates = new[] { At(7), At(8, 1), At(9, 2), At(10, 3), At(15, 30), At(16, 31), At(7).AddDays(1) };
        Assert.Throws<ArgumentException>(() => calculator.CalculateDailyFee(car, dates));
        Assert.Throws<ArgumentException>(() => calculator.CalculateDailyFee(VehicleType.Motorbike, dates));
    }

    [Fact]
    public void Null_arguments_are_rejected()
    {
        Assert.Throws<ArgumentNullException>(() => new TollCalculator(null!));
        Assert.Throws<ArgumentNullException>(() => calculator.CalculateDailyFee(car, null!));
    }

    [Theory]
    [InlineData(VehicleType.Unknown)]
    [InlineData((VehicleType)(-1))]
    [InlineData((VehicleType)999)]
    public void Unrecognized_categories_are_rejected_even_on_free_days_or_empty_input(VehicleType vehicle)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.GetPassageFee(vehicle, At(7)));
        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.GetPassageFee(vehicle, new DateTime(2013, 1, 1)));
        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.CalculateDailyFee(vehicle, Array.Empty<DateTime>()));
    }

    [Theory]
    [InlineData(DateTimeKind.Utc)]
    [InlineData(DateTimeKind.Local)]
    public void Timestamp_kind_must_explicitly_represent_city_wall_time(DateTimeKind kind)
    {
        DateTime date = DateTime.SpecifyKind(At(7), kind);
        Assert.Throws<ArgumentException>(() => calculator.GetPassageFee(car, date));
        Assert.Throws<ArgumentException>(() => calculator.CalculateDailyFee(car, new[] { date }));
    }

    [Fact]
    public void Unsupported_calendar_year_is_rejected()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.GetPassageFee(car, At(7).AddYears(1)));
        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.CalculateDailyFee(VehicleType.Motorbike, new[] { At(7).AddYears(1) }));
    }

    [Fact]
    public void Randomized_inputs_obey_order_duplicate_and_cap_invariants()
    {
        var random = new Random(42);
        for (int sample = 0; sample < 200; sample++)
        {
            DateTime[] dates = Enumerable.Range(0, random.Next(1, 50))
                .Select(_ => At(0).AddSeconds(random.Next(24 * 60 * 60))).ToArray();
            int fee = calculator.CalculateDailyFee(car, dates);
            Assert.InRange(fee, 0, 60);
            Assert.Equal(fee, calculator.CalculateDailyFee(car, dates.Reverse().ToArray()));
            Assert.Equal(fee, calculator.CalculateDailyFee(car, dates.Concat(dates).ToArray()));
        }
    }

    [Fact]
    public void Lazy_input_is_enumerated_once()
    {
        int enumerations = 0;
        IEnumerable<DateTime> Passages()
        {
            if (++enumerations > 1) throw new InvalidOperationException("Input read twice.");
            yield return At(7);
            yield return At(6);
        }

        Assert.Equal(18, calculator.CalculateDailyFee(car, Passages()));
        Assert.Equal(1, enumerations);
    }

    [Fact]
    public void Duplicate_final_passage_does_not_close_the_window_twice()
        => Assert.Equal(26, calculator.CalculateDailyFee(car, new[] { At(6), At(7, 1), At(7, 1) }));

    [Fact]
    public void Passages_across_a_clock_hour_still_share_one_window()
        => Assert.Equal(18, calculator.CalculateDailyFee(car, new[] { At(6, 59), At(7, 1) }));

    [Fact]
    public void Validation_does_not_depend_on_vehicle_or_date_exemptions()
    {
        DateTime holiday = new(2013, 1, 1, 7, 0, 0);
        Assert.Throws<ArgumentException>(() => calculator.CalculateDailyFee(car,
            new[] { holiday, holiday.AddDays(1) }));
        Assert.Throws<ArgumentException>(() => calculator.CalculateDailyFee(VehicleType.Motorbike,
            new[] { DateTime.SpecifyKind(At(7), DateTimeKind.Utc) }));
    }
}
