using Microsoft.Extensions.Options;
using Afry.TollCalculator.Models;
using Afry.TollCalculator.Infrastructure.Settings;
using Afry.TollCalculator.Infrastructure;
using Afry.TollCalculator.Models;

namespace Afry.TollCalculator.Services;

public sealed class JsonTollFeeScheduleProvider : ITollFeeScheduleProvider
{
    private readonly TollSettings _settings;
    private readonly Money _minFee;
    private readonly Money _maxFee;
    private readonly Lazy<Result<IReadOnlyList<TollFeeTimeRange>>> _cached;

    public JsonTollFeeScheduleProvider(IOptions<TollSettings> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _settings = options.Value;

        if (_settings.FeeMinMinorUnit < 0 || _settings.FeeMaxMinorUnit < 0)
        {
            throw new InvalidOperationException("Toll settings must include non-negative feeMinMinorUnit and feeMaxMinorUnit.");
        }

        if (_settings.FeeMinMinorUnit > _settings.FeeMaxMinorUnit)
        {
            throw new InvalidOperationException("Toll settings feeMinMinorUnit must be <= feeMaxMinorUnit.");
        }

        _minFee = new Money(_settings.FeeMinMinorUnit, _settings.Currency);
        _maxFee = new Money(_settings.FeeMaxMinorUnit, _settings.Currency);
        _cached = new Lazy<Result<IReadOnlyList<TollFeeTimeRange>>>(() => GetTollFeeTimeRange(_settings, _minFee, _maxFee));
    }

    public Result<IReadOnlyList<TollFeeTimeRange>> GetSchedule() => _cached.Value;

    private static Result<IReadOnlyList<TollFeeTimeRange>> GetTollFeeTimeRange(TollSettings settings, Money minFee, Money maxFee)
    {
        var ranges = settings.FeeSchedule
            .Select(i => new TollFeeTimeRange(
                TimeOnly.Parse(i.From),
                TimeOnly.Parse(i.To),
                new Money(i.FeeMinorUnit, settings.Currency)))
            .OrderBy(r => r.From)
            .ToArray();

        for (int i = 0; i < ranges.Length; i++)
        {
            if (ranges[i].From > ranges[i].To)
            {
                return Result<IReadOnlyList<TollFeeTimeRange>>.Failure($"Invalid fee schedule: From '{ranges[i].From}' is after To '{ranges[i].To}'.");
            }

            if (ranges[i].Fee < minFee || ranges[i].Fee > maxFee)
            {
                return Result<IReadOnlyList<TollFeeTimeRange>>.Failure($"Invalid fee schedule: fee '{ranges[i].Fee}' is outside allowed range {minFee}..{maxFee}.");
            }
        }

        for (int i = 1; i < ranges.Length; i++)
        {
            if (ranges[i].From <= ranges[i - 1].To)
            {
                return Result<IReadOnlyList<TollFeeTimeRange>>.Failure($"Invalid fee schedule: overlapping ranges '{ranges[i - 1].From}-{ranges[i - 1].To}' and '{ranges[i].From}-{ranges[i].To}'.");
            }
        }

        return Result<IReadOnlyList<TollFeeTimeRange>>.Success(ranges);
    }
}
