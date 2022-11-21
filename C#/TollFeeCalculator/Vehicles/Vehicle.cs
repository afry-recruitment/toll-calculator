namespace TollFeeCalculator.Vehicles
{
    public class Vehicle : IVehicle
    {
        public string VehicleId { get; }
        public VehicleType VehicleType { get; set; }
        public VehicleClassification VehicleClassification { get; set; }

        public Vehicle(string vehicleId)
        {
            VehicleId = vehicleId;
            VehicleType = VehicleType.UnTyped;
            VehicleClassification = VehicleClassification.Standard;
        }

        public Vehicle(
            string vehicleId,
            VehicleType vehicleType = VehicleType.UnTyped,
            VehicleClassification vehicleClassification = VehicleClassification.Standard)
        {
            VehicleId = vehicleId;
            VehicleType = vehicleType;
            VehicleClassification = vehicleClassification;
        }
    }
}
