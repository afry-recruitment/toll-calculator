using TollFeeCalculator.Interfaces;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Services;

public class VehicleService(VehicleConfiguration config) : IVehicleService
{
    private readonly HashSet<string> _tollFreeVehicles = 
        new(config.TollFreeVehicleTypes, StringComparer.OrdinalIgnoreCase);
    
    public bool IsTollFree(string vehicleType) =>
        !string.IsNullOrWhiteSpace(vehicleType) && 
        _tollFreeVehicles.Contains(vehicleType);
}