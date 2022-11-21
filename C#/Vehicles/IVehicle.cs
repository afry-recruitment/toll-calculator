namespace TollFeeCalculator.Vehicles
{
    public interface IVehicle
    {
        VehicleType VehicleType { get; }

        VehicleClassification VehicleClassification { get; }
        
    }
}