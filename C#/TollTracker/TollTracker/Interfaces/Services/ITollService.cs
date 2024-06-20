using TollTracker.Enums;

namespace TollTracker.Interfaces.Services;

public interface ITollService
{
    Task<bool> IsDateExemptFromTollAsync(DateTime date, CancellationToken cancellationToke);

    bool IsVehicleTypeExemptFromToll(VehicleType type);

    int CalculateVehicleTollFee(string registrationNumber, IEnumerable<DateTime> Timestamps);
}

