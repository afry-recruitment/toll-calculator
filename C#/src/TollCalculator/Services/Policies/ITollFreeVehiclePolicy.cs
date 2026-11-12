using TollCalculator.Models;

namespace TollCalculator.Services.Policies;

public interface ITollFreeVehiclePolicy
{
    bool IsTollFree(IVehicle vehicle);
}