namespace TollCalculator.Models
{
    // Made this an abstract class instead of an interface to improve scaleability incase of future use
    public abstract class Vehicle
    {
        // LicensePlate needs to be unique, but for simplicities sake I ensure that by sending it in as a parameter in the constructor
        public string LicensePlate { get; protected set; }
        public VehicleType Type { get; protected set; }
        public bool IsTollFree { get; private set; }

        public Vehicle(string licensePlate)
        {
            LicensePlate = licensePlate;
            IsTollFree = Enum.IsDefined(typeof(TollFreeVehicles), Type);
        }

        public Vehicle(string licensePlate, VehicleType type) 
        {
            LicensePlate = licensePlate;
            Type = type;
            IsTollFree = Enum.IsDefined(typeof(TollFreeVehicles), Type);
        }

        public enum VehicleType
        {
            Motorbike = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5,
            Car = 6
        }

        private enum TollFreeVehicles
        {
            Motorbike = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5
        }
    }
}
