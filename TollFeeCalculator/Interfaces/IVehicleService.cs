namespace TollFeeCalculator.Interfaces;

public interface IVehicleService
{
    bool IsTollFree(string vehicleType);
}