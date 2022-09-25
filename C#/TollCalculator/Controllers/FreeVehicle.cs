using TollCalculator.Models;

namespace TollCalculator.Controller;
public class FreeVehicle
{
    public bool IsTollFreeVehicle(Vehicle vehicle)
    {
        var vehicleType = vehicle.GetVehicleType();
        return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
    }

    public bool IsTollFreeDate(DateTime date)
    {
        //This list can be fetched from an API 
        var holidays = new List<DateTime> { new DateTime(2022, 1, 1), new DateTime(2022, 12, 25) };

        return date.DayOfWeek switch
        {
            DayOfWeek.Saturday => true,
            DayOfWeek.Sunday => true,
            _ => holidays.Contains(date) ? true : false
        };

    }
}