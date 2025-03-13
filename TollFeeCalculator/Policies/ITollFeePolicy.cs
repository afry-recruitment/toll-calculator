using TollFeeCalculator.Models;

namespace TollFeeCalculator.Policies;

public interface ITollFeePolicy
{
    string Currency { get; }
    decimal DailyMaxFee { get; }
    TollFeeRate[] TollFeeRates { get; }
    bool IsTollFreeDate(DateOnly date);
    bool IsTollFreeVehicleType(VehicleType vehicleType);
}