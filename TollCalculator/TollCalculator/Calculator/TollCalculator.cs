using Tollculator.Services;
using Tollculator.WorkingDayChecker;

namespace Tollculator.Calculator;

public class TollCalculator : ITollCalculator
{
    private readonly IWorkingDayChecker _workingDayChecker;
    private readonly IVehicleService _vehicleService;

    public TollCalculator(IWorkingDayChecker workingDayChecker, IVehicleService vehicleService)
    {
        _workingDayChecker = workingDayChecker;
        _vehicleService = vehicleService;
    }

    /// <summary>
    /// Calculate the toll fee for a car on a specific day.
    /// </summary>
    /// <param name="date">The day of the current toll fee calculation.</param>
    /// <param name="registrationNumber">The registration number of the car.</param>
    /// <param name="times">The times the vehicle passed the toll.</param>
    /// <returns>The total toll fee.</returns>
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> GetTollFee(DateOnly date, string registrationNumber, IList<TimeOnly> times)
    {
        if (await _workingDayChecker.IsTollFree(date) || _vehicleService.IsTollFree(registrationNumber))
        {
            return 0;
        }

        var i = 0;
        var tollFee = 0;

        while (i < times.Count)
        {
            var fromTemp = times[i];
            var toTemp = fromTemp.AddHours(1);

            if (fromTemp > toTemp)
            {
                toTemp = new TimeOnly(23, 59);
            }

            var timesWithinRange = times.Where(time => time >= fromTemp && time < toTemp || time == fromTemp && time == toTemp).ToList();

            tollFee += TollCalculatorHelper.GetHighestTollFromTimes(timesWithinRange);

            i += timesWithinRange.Count;
        }

        return tollFee > 60 ? 60 : tollFee;
    }
}