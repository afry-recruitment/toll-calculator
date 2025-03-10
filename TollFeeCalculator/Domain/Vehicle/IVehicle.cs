namespace TollFeeCalculator.Domain.Vehicle
{
    public interface IVehicle
    {
        VehicleType GetVehicleType();
    }

    public enum VehicleType
    {
        Car,
        Motorbike,
        Tractor,
        Emergency,
        Diplomat,
        Foreign,
        Military
    }
}