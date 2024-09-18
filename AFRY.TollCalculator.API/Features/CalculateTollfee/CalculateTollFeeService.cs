using AFRY.TollCalculator.API.Domain.Enums;
using AFRY.TollCalculator.API.Domain.Models;

namespace AFRY.TollCalculator.API.Features.CalculateTollfee;

public class CalculateTollFeeService(ICalculateTollFeeRepository calculateTollFeeRepository) : ICalculateTollFeeService
{
    private readonly ICalculateTollFeeRepository _calculateTollFeeRepository = calculateTollFeeRepository;

    public int GetTotalTollFeeForMultipleDays(Vehicle vehicle, DateTime[] dates)
    {
        var groupedDatesByDay = dates.GroupBy(d => d.Date);
        int totalFeeForAllDays = 0;

        foreach (var dayGroup in groupedDatesByDay)
        {
            DateTime[] datesForDay = dayGroup.ToArray();
            int dailyTollFee = GetTollFee(vehicle, datesForDay);
            totalFeeForAllDays += dailyTollFee;
        }

        return totalFeeForAllDays;
    }

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        int highestFeeInCurrentInterval = 0;

        foreach (DateTime date in dates)
        {
            int currentFee = GetTollFee(date, vehicle);

            TimeSpan timeDiff = date - intervalStart;
            double minutes = timeDiff.TotalMinutes;

            if (minutes < 60)
            {
                highestFeeInCurrentInterval = Math.Max(highestFeeInCurrentInterval, currentFee);
            }
            else
            {
                totalFee += highestFeeInCurrentInterval;
                intervalStart = date;
                highestFeeInCurrentInterval = currentFee;
            }

            if (totalFee + highestFeeInCurrentInterval >= 60)
            {
                return 60;
            }
        }

        totalFee += highestFeeInCurrentInterval;

        return Math.Min(totalFee, 60);
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        TimeSpan time = date.TimeOfDay;

        var tollFeePeriod = _calculateTollFeeRepository.GetTollFeePeriod(time);

        return tollFeePeriod != null ? tollFeePeriod.Fee : 0;
    }

    public bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        return _calculateTollFeeRepository.IsDateTollFree(date.Date);
    }

    public bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;

        if (Enum.TryParse(vehicle.GetVehicleType(), out TollFreeVehicles vehicleType))
        {
            return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
        }

        return false;
    }
}
