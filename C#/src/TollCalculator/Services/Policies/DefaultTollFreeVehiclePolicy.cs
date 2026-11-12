using TollCalculator.Models;

namespace TollCalculator.Services.Policies;

public sealed class DefaultTollFreeVehiclePolicy : ITollFreeVehiclePolicy
{
    public bool IsTollFree(IVehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);

        return Enum.TryParse<TollFreeVehicles>(
            vehicle.VehicleType.ToString(),
            ignoreCase: true,
            out _);
    }
}