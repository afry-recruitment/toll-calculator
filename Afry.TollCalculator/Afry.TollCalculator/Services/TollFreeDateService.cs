using Microsoft.Extensions.Options;
using Afry.TollCalculator.Infrastructure.Settings;
using Afry.TollCalculator.Infrastructure;

namespace Afry.TollCalculator.Services;

public sealed class TollFreeDateService : ITollFreeDateService
{
    private sealed record ParsedSettings(HashSet<DayOfWeek> TollFreeWeekdays, HashSet<DateOnly> TollFreeDays);

    private readonly Lazy<Result<ParsedSettings>> _parsed;

    public TollFreeDateService(IOptions<TollSettings> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        var settings = options.Value;
        _parsed = new Lazy<Result<ParsedSettings>>(() => Parse(settings));
    }

    public Result<bool> IsTollFreeDate(DateTimeOffset date)
    {
        var parsedResult = _parsed.Value;
        if (!parsedResult.IsSuccess)
        {
            return Result<bool>.Failure(parsedResult.Error!);
        }

        var parsed = parsedResult.Value!;

        var local = date.LocalDateTime;

        if (parsed.TollFreeWeekdays.Contains(local.DayOfWeek))
        {
            return Result<bool>.Success(true);
        }

        if (parsed.TollFreeDays.Contains(DateOnly.FromDateTime(local)))
        {
            return Result<bool>.Success(true);
        }

        return Result<bool>.Success(false);
    }

    private static Result<ParsedSettings> Parse(TollSettings settings)
    {
        HashSet<DayOfWeek> weekdays = [];
        foreach (var w in settings.TollFreeWeekdays)
        {
            if (!Enum.TryParse<DayOfWeek>(w, ignoreCase: true, out var dayOfWeek))
            {
                return Result<ParsedSettings>.Failure($"Invalid toll-free weekday: '{w}'.");
            }

            weekdays.Add(dayOfWeek);
        }

        HashSet<DateOnly> days = [];
        foreach (var d in settings.TollFreeDays)
        {
            if (!DateOnly.TryParse(d, out var date))
            {
                return Result<ParsedSettings>.Failure($"Invalid toll-free day: '{d}'. Use ISO format 'yyyy-MM-dd'.");
            }

            days.Add(date);
        }

        return Result<ParsedSettings>.Success(new ParsedSettings(weekdays, days));
    }
}
