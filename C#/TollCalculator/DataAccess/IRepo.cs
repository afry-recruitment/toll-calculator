namespace TollCalculator.DataAccess;

public interface IRepo
{
    bool IsDayOff(DateTime date);
    bool IsVehicleTollFree(string vehicleType);
}