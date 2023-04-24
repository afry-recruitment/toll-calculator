namespace TollCalculator.Models
{
    // Made an abstract class instead of an interface to increase flexibility and makes adding new vehicles easy
    public abstract class Vehicle
    {
        // LicensePlate needs to be unique, but for simplicities sake I ensure that by sending it in as a parameter in the constructor
        public string LicensePlate { get; protected set; }
        public VehicleSector Sector { get; protected set; }
        public bool IsTollFree { get; private set; }

        public Vehicle(string licensePlate, VehicleSector sector) 
        {
            LicensePlate = licensePlate;
            Sector = sector;

            // true if the vehicles sector or the name of the derived class exists in TollFreeVehicle
            IsTollFree = Enum.IsDefined(typeof(TollFreeVehicle), Sector.ToString())
                      || Enum.IsDefined(typeof(TollFreeVehicle), GetType().Name);
        }

        public enum VehicleSector
        {
            Civilian = 0,
            Emergency = 1,
            Diplomat = 2,
            Foreign = 3,
            Military = 4
        }

        // Here you can decide what you want the toll-free vehicles to be
        // It either needs to be included in the VehicleSector above or
        // be the class name of a new derived Vehicle
        private enum TollFreeVehicle
        {
            Motorbike = 0,
            Emergency = 1,
            Diplomat = 2,
            Foreign = 3,
            Military = 4
        }
    }
}
