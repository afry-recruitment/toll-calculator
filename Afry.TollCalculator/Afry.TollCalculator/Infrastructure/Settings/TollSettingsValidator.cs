using Microsoft.Extensions.Options;
using Afry.TollCalculator.Models;

namespace Afry.TollCalculator.Infrastructure.Settings;

public sealed class TollSettingsValidator : IValidateOptions<TollSettings>
{
    public ValidateOptionsResult Validate(string? name, TollSettings options)
    {
        if (options is null)
        {
            return ValidateOptionsResult.Fail("TollSettings is null.");
        }

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.Currency))
        {
            errors.Add("TollSettings.currency must be specified.");
        }

        if (options.MaxDailyFeeMinorUnit < 0)
        {
            errors.Add("TollSettings.maxDailyFeeMinorUnit must be non-negative.");
        }

        if (options.FeeMinMinorUnit < 0)
        {
            errors.Add("TollSettings.feeMinMinorUnit must be non-negative.");
        }

        if (options.FeeMaxMinorUnit < 0)
        {
            errors.Add("TollSettings.feeMaxMinorUnit must be non-negative.");
        }

        if (options.FeeMinMinorUnit > options.FeeMaxMinorUnit)
        {
            errors.Add("TollSettings.feeMinMinorUnit must be <= feeMaxMinorUnit.");
        }

        options.TollFreeVehicles ??= [];
        foreach (var v in options.TollFreeVehicles)
        {
            if (string.IsNullOrWhiteSpace(v) || !Enum.TryParse<VehicleType>(v, ignoreCase: true, out _))
            {
                errors.Add($"Invalid toll-free vehicle type: '{v}'.");
            }
        }

        options.TollFreeWeekdays ??= [];
        foreach (var w in options.TollFreeWeekdays)
        {
            if (string.IsNullOrWhiteSpace(w) || !Enum.TryParse<DayOfWeek>(w, ignoreCase: true, out _))
            {
                errors.Add($"Invalid toll-free weekday: '{w}'.");
            }
        }

        options.TollFreeHours ??= [];
        foreach (var r in options.TollFreeHours)
        {
            if (!TimeOnly.TryParse(r.From, out var from) || !TimeOnly.TryParse(r.To, out var to))
            {
                errors.Add($"Invalid tollFreeHours range: '{r.From}-{r.To}'.");
                continue;
            }

            if (from > to)
            {
                errors.Add($"Invalid tollFreeHours range: From '{r.From}' is after To '{r.To}'.");
            }
        }

        options.TollFreeDays ??= [];
        foreach (var d in options.TollFreeDays)
        {
            if (string.IsNullOrWhiteSpace(d) || !DateOnly.TryParse(d, out _))
            {
                errors.Add($"Invalid toll-free day: '{d}'. Use ISO format 'yyyy-MM-dd'.");
            }
        }

        options.FeeSchedule ??= [];
        foreach (var f in options.FeeSchedule)
        {
            if (!TimeOnly.TryParse(f.From, out var from) || !TimeOnly.TryParse(f.To, out var to))
            {
                errors.Add($"Invalid feeSchedule range: '{f.From}-{f.To}'.");
                continue;
            }

            if (from > to)
            {
                errors.Add($"Invalid feeSchedule range: From '{f.From}' is after To '{f.To}'.");
            }

            if (f.FeeMinorUnit < 0)
            {
                errors.Add($"Invalid feeSchedule feeMinorUnit: '{f.FeeMinorUnit}'.");
            }
        }

        return errors.Count == 0 ? ValidateOptionsResult.Success : ValidateOptionsResult.Fail(errors);
    }
}
