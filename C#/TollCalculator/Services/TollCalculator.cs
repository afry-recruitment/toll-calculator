using System;
using System.Globalization;
using TollCalculator.Models;

namespace TollCalculator.Services;

public interface ITollCalculator
{
    decimal CalculateTollFee(Vehicle vehicle, IEnumerable<DateTime> passageTimes);
}

public class TollCalculator : ITollCalculator
{
    //These business rules might be better to move to configuration.
    private readonly HashSet<VehicleType> _tollfreeVehicles = new HashSet<VehicleType> {
        VehicleType.Motorbike,
        VehicleType.Tractor,
        VehicleType.Emergency,
        VehicleType.Diplomat,
        VehicleType.Foreign,
        VehicleType.Military
    };
    private readonly IDateService _dateService;

    public TollCalculator(IDateService dateService)
    {
        _dateService = dateService;
    }

    public decimal CalculateTollFee(Vehicle vehicle, IEnumerable<DateTime> passageTimes)
    {
        if (passageTimes.Count() == 0)
        {
            throw new ArgumentException("There must be at least one passage time stamp");
        }

        if (IsTollFreeVehicle(vehicle))
        {
            return 0;
        }

        var passageTimesPerDay = passageTimes.GroupBy(x => x.Date);
        decimal totalFee = 0;
        foreach (var passageTimesForOneDay in passageTimesPerDay)
        {
            if (IsTollFreeDate(passageTimesForOneDay.First()))
            {
                continue;
            }
            var passageTimeStamps = passageTimesForOneDay.Select(x => TimeOnly.FromDateTime(x));
            totalFee += CalculateTollFeeForOneDay(passageTimeStamps);
        }

        return totalFee;
    }
    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        return _tollfreeVehicles.Contains(vehicle.VehicleType);
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        if (_dateService.IsHoliday(date))
        {
            return true;
        }

        return false;
    }

    //This would be more readable and easier to debug if it was rewritten using a model for calculations. Not enough time.
    private decimal CalculateTollFeeForOneDay(IEnumerable<TimeOnly> timeStamps)
    {
        var orderedTimeStamps = timeStamps.OrderBy(x => x).ToList();

        decimal CalculateFee(List<TimeOnly> timestamps, TimeOnly beginningOfHourlyPeriod, decimal hourlyFee, decimal totalFee)
        {
            return timestamps switch
            {
                [] => totalFee + hourlyFee,
                [var nextTimeStamp, .. var rest] when (nextTimeStamp - beginningOfHourlyPeriod).TotalMinutes <= 60 =>
                    CalculateFee(rest, beginningOfHourlyPeriod, Math.Max(hourlyFee, GetTollFee(nextTimeStamp)), totalFee),
                [var nextTimeStamp, .. var rest] =>
                    CalculateFee(rest, nextTimeStamp, GetTollFee(nextTimeStamp), totalFee + hourlyFee)
            };
        }

        var calculatedFee = CalculateFee(orderedTimeStamps, orderedTimeStamps.First(), GetTollFee(orderedTimeStamps.First()), 0);
        return Math.Min(calculatedFee, 60m);
    }

    //This business logic might be better to move to configuration.
    public int GetTollFee(TimeOnly timeStamp)
    {
        return timeStamp switch
        {
            { Hour: 6, Minute: >= 0, Minute: <= 29 } => 8,
            { Hour: 6, Minute: >= 30, Minute: <= 59 } => 13,
            { Hour: 7, Minute: >= 0, Minute: <= 59 } => 18,
            { Hour: 8, Minute: >= 0, Minute: <= 29 } => 13,
            { Hour: >= 8, Hour: <= 14, Minute: >= 30, Minute: <= 59 } => 8,
            { Hour: 15, Minute: >= 0, Minute: <= 29 } => 13,
            { Hour: 15, Minute: >= 0 } or { Hour: 16, Minute: <= 59 } => 18,
            { Hour: 17, Minute: >= 0, Minute: <= 59 } => 13,
            { Hour: 18, Minute: >= 0, Minute: <= 29 } => 8,
            _ => 0
        };
    }
}