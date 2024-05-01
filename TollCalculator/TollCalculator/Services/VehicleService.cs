using Tollculator.VehicleRegister;

namespace Tollculator.Services;

public class VehicleService : IVehicleService
{
    /// <summary>
    /// Check if the vehicle is toll free by registrationNumber.
    /// </summary>
    /// <param name="registrationNumber">The registration number of the vechicle to check.</param>
    /// <returns><c>true</c> if the car is toll free.</returns>
    public bool IsTollFree(string registrationNumber)
    {
        var isTollFree = false;
        // If registration number not found we assume that toll should be applied.
        if (Register.Vehicles.TryGetValue(registrationNumber, out var vehicleType))
        {
            isTollFree = Register.TollFreeVechicleTypes.Any(x => x == vehicleType);
        }

        return isTollFree;
    }
}
