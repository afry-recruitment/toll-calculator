namespace Tollculator.Services;

public interface IVehicleService
{
    bool IsTollFree(string registrationNumber);
}
