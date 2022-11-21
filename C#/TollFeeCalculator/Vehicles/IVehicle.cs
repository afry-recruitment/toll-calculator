namespace TollFeeCalculator.Vehicles
{
    public interface IVehicle
    {
        string VehicleId { get; }

        VehicleType VehicleType { get; set; }

        VehicleClassification VehicleClassification { get; set; }
    }
}