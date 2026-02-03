using System.Text.Json;
using Afry.TollCalculator.Models;

namespace Afry.TollCalculator.Infrastructure.Settings;

public sealed class TollSettings
{
    public string Currency { get; set; } = "";
    public long MaxDailyFeeMinorUnit { get; set; }
    public long FeeMinMinorUnit { get; set; }
    public long FeeMaxMinorUnit { get; set; }
    public List<string> TollFreeVehicles { get; set; } = [];
    public List<string> TollFreeWeekdays { get; set; } = [];
    public List<TollTimeRange> TollFreeHours { get; set; } = [];
    public List<string> TollFreeDays { get; set; } = [];
    public List<TollFeeScheduleItem> FeeSchedule { get; set; } = [];

    public static TollSettings Load(string? path = null)
    {
        var settingsPath = path ?? Path.Combine(AppContext.BaseDirectory, "tollSettings.json");

        if (!File.Exists(settingsPath))
        {
            throw new FileNotFoundException($"Toll settings file not found: '{settingsPath}'.", settingsPath);
        }

        var json = File.ReadAllText(settingsPath);
        var settings = JsonSerializer.Deserialize<TollSettings>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (settings is null)
        {
            throw new InvalidOperationException("Toll settings file is invalid.");
        }

        if (string.IsNullOrWhiteSpace(settings.Currency))
        {
            throw new InvalidOperationException("Toll settings must specify a currency.");
        }

        if (settings.MaxDailyFeeMinorUnit < 0)
        {
            throw new InvalidOperationException("Toll settings maxDailyFeeMinorUnit must be non-negative.");
        }

        if (settings.FeeMinMinorUnit < 0)
        {
            throw new InvalidOperationException("Toll settings feeMinMinorUnit must be non-negative.");
        }

        if (settings.FeeMaxMinorUnit < 0)
        {
            throw new InvalidOperationException("Toll settings feeMaxMinorUnit must be non-negative.");
        }

        if (settings.FeeMinMinorUnit > settings.FeeMaxMinorUnit)
        {
            throw new InvalidOperationException("Toll settings feeMinMinorUnit must be <= feeMaxMinorUnit.");
        }

        settings.TollFreeVehicles ??= [];
        settings.TollFreeWeekdays ??= [];
        settings.TollFreeHours ??= [];
        settings.TollFreeDays ??= [];
        settings.FeeSchedule ??= [];

        if (settings.TollFreeVehicles is null)
        {
            throw new InvalidOperationException("Toll settings must include tollFreeVehicles.");
        }

        foreach (var v in settings.TollFreeVehicles)
        {
            if (string.IsNullOrWhiteSpace(v) || !Enum.TryParse<VehicleType>(v, ignoreCase: true, out _))
            {
                throw new InvalidOperationException($"Invalid toll-free vehicle type: '{v}'.");
            }
        }

        if (settings.TollFreeWeekdays is null)
        {
            throw new InvalidOperationException("Toll settings must include tollFreeWeekdays.");
        }

        foreach (var w in settings.TollFreeWeekdays)
        {
            if (string.IsNullOrWhiteSpace(w) || !Enum.TryParse<DayOfWeek>(w, ignoreCase: true, out _))
            {
                throw new InvalidOperationException($"Invalid toll-free weekday: '{w}'.");
            }
        }

        if (settings.TollFreeHours is null)
        {
            throw new InvalidOperationException("Toll settings must include tollFreeHours.");
        }

        foreach (var r in settings.TollFreeHours)
        {
            if (!TimeOnly.TryParse(r.From, out var from) || !TimeOnly.TryParse(r.To, out var to))
            {
                throw new InvalidOperationException($"Invalid tollFreeHours range: '{r.From}-{r.To}'.");
            }

            if (from > to)
            {
                throw new InvalidOperationException($"Invalid tollFreeHours range: From '{r.From}' is after To '{r.To}'.");
            }
        }

        if (settings.TollFreeDays is null)
        {
            throw new InvalidOperationException("Toll settings must include tollFreeDays.");
        }

        foreach (var d in settings.TollFreeDays)
        {
            if (string.IsNullOrWhiteSpace(d) || !DateOnly.TryParse(d, out _))
            {
                throw new InvalidOperationException($"Invalid toll-free day: '{d}'. Use ISO format 'yyyy-MM-dd'.");
            }
        }

        if (settings.FeeSchedule is null)
        {
            throw new InvalidOperationException("Toll settings must include feeSchedule.");
        }

        foreach (var f in settings.FeeSchedule)
        {
            if (!TimeOnly.TryParse(f.From, out var from) || !TimeOnly.TryParse(f.To, out var to))
            {
                throw new InvalidOperationException($"Invalid feeSchedule range: '{f.From}-{f.To}'.");
            }

            if (from > to)
            {
                throw new InvalidOperationException($"Invalid feeSchedule range: From '{f.From}' is after To '{f.To}'.");
            }

            if (f.FeeMinorUnit < 0)
            {
                throw new InvalidOperationException($"Invalid feeSchedule feeMinorUnit: '{f.FeeMinorUnit}'.");
            }
        }

        return settings;
    }
}

public sealed record TollTimeRange(string From, string To);

public sealed record TollFeeScheduleItem(string From, string To, long FeeMinorUnit);
