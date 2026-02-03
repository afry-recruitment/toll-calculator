using Afry.TollCalculator.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Afry.TollCalculator.Infrastructure.Settings;
using Afry.TollCalculator.Infrastructure;
using Afry.TollCalculator.Models;

namespace Afry.TollCalculator.Services;

public sealed class TollCalculatorService : ITollCalculatorService
{
    private readonly TollSettings _settings;
    private readonly Money _maxDailyFee;
    private readonly (TimeOnly From, TimeOnly To)[] _tollFreeHours;
    private readonly HashSet<VehicleType> _tollFreeVehicleTypes;
    private readonly ITollFreeDateService _tollFreeDateService;
    private readonly ITollFeeScheduleProvider _feeScheduleProvider;
    private readonly ILogger<TollCalculatorService> _logger;

    public TollCalculatorService(
        IOptions<TollSettings> options,
        ILogger<TollCalculatorService> logger,
        ITollFreeDateService tollFreeDateService,
        ITollFeeScheduleProvider feeScheduleProvider)
    {
        _logger = logger;
        _settings = options.Value;
        _maxDailyFee = new Money(_settings.MaxDailyFeeMinorUnit, _settings.Currency);
        _tollFreeHours = [.. _settings.TollFreeHours.Select(r => (TimeOnly.Parse(r.From), TimeOnly.Parse(r.To)))];
        _tollFreeVehicleTypes = [.. _settings.TollFreeVehicles.Select(v => Enum.Parse<VehicleType>(v, ignoreCase: true))];
        _tollFreeDateService = tollFreeDateService;
        _feeScheduleProvider = feeScheduleProvider;
    }

    /// <summary>
    /// Calculates the total toll fee for one local day.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="dates">All pass timestamps for the same local day.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the calculated <see cref="Money"/>.
    /// When the input is invalid, returns a failure with an error message.
    /// </returns>
    public Result<Money> GetTollFee(Vehicle vehicle, DateTimeOffset[] dates)
    {
        if (vehicle is null)
        {
            _logger.LogError("GetTollFee called with null vehicle.");
            return Result<Money>.Failure("Vehicle must be provided.");
        }

        if (dates is null || dates.Length == 0)
        {
            _logger.LogError("GetTollFee called with null or empty dates.");
            return Result<Money>.Failure("At least one pass timestamp must be provided.");
        }

        var orderedDates = dates.OrderBy(d => d).ToArray();

        var day = DateOnly.FromDateTime(orderedDates[0].LocalDateTime);
        for (int i = 1; i < orderedDates.Length; i++)
        {
            if (DateOnly.FromDateTime(orderedDates[i].LocalDateTime) != day)
            {
                return Result<Money>.Failure("All passes must be on the same local date.");
            }
        }

        var intervalStart = orderedDates[0];

        var scheduleResult = _feeScheduleProvider.GetSchedule();
        if (!scheduleResult.IsSuccess)
        {
            return Result<Money>.Failure(scheduleResult.Error!);
        }

        var feeSchedule = scheduleResult.Value!;

        var intervalMaxFeeResult = GetTollFeeMoney(intervalStart, vehicle, feeSchedule);
        if (!intervalMaxFeeResult.IsSuccess)
        {
            return Result<Money>.Failure(intervalMaxFeeResult.Error!);
        }

        var intervalMaxFee = intervalMaxFeeResult.Value!;
        var totalFee = intervalMaxFee;

        for (int i = 1; i < orderedDates.Length; i++)
        {
            var date = orderedDates[i];
            var nextFeeResult = GetTollFeeMoney(date, vehicle, feeSchedule);
            if (!nextFeeResult.IsSuccess)
            {
                return Result<Money>.Failure(nextFeeResult.Error!);
            }

            var nextFee = nextFeeResult.Value!;

            var minutes = (date - intervalStart).TotalMinutes;
            if (minutes <= 60)
            {
                if (nextFee > intervalMaxFee)
                {
                    totalFee += nextFee - intervalMaxFee;
                    intervalMaxFee = nextFee;
                }
            }
            else
            {
                intervalStart = date;
                intervalMaxFee = nextFee;
                totalFee += nextFee;
            }

            if (totalFee >= _maxDailyFee)
            {
                return Result<Money>.Success(_maxDailyFee);
            }
        }

        var capped = totalFee < _maxDailyFee ? totalFee : _maxDailyFee;
        return Result<Money>.Success(capped);
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
            return false;

        return _tollFreeVehicleTypes.Contains(vehicle.VehicleType);
    }

    /// <summary>
    /// Calculates the toll fee for a single pass.
    /// </summary>
    /// <param name="date">The pass timestamp.</param>
    /// <param name="vehicle">The vehicle.</param>
    /// <returns>A <see cref="Result{T}"/> containing the calculated <see cref="Money"/>.</returns>
    public Result<Money> GetTollFee(DateTimeOffset date, Vehicle vehicle)
    {
        if (vehicle is null)
        {
            return Result<Money>.Success(Money.Zero(_settings.Currency));
        }

        var scheduleResult = _feeScheduleProvider.GetSchedule();
        if (!scheduleResult.IsSuccess)
        {
            return Result<Money>.Failure(scheduleResult.Error!);
        }

        var feeResult = GetTollFeeMoney(date, vehicle, scheduleResult.Value!);
        if (!feeResult.IsSuccess)
        {
            return Result<Money>.Failure(feeResult.Error!);
        }

        return Result<Money>.Success(feeResult.Value!);
    }

    private Result<Money> GetTollFeeMoney(DateTimeOffset date, Vehicle vehicle, IReadOnlyList<TollFeeTimeRange> feeSchedule)
    {
        var tollFreeDateResult = _tollFreeDateService.IsTollFreeDate(date);
        if (!tollFreeDateResult.IsSuccess)
        {
            return Result<Money>.Failure(tollFreeDateResult.Error!);
        }

        if (IsTollFreeVehicle(vehicle) || tollFreeDateResult.Value == true)
        {
            return Result<Money>.Success(Money.Zero(_settings.Currency));
        }

        var time = TimeOnly.FromDateTime(date.LocalDateTime);

        foreach (var (From, To) in _tollFreeHours)
        {
            if (time >= From && time <= To)
            {
                return Result<Money>.Success(Money.Zero(_settings.Currency));
            }
        }

        foreach (var range in feeSchedule)
        {
            if (range.Contains(time))
            {
                return Result<Money>.Success(range.Fee);
            }
        }

        return Result<Money>.Success(Money.Zero(_settings.Currency));
    }
}