namespace TollCalculator.Models
{
    // Made an abstract class instead of an interface to increase flexibility and makes adding new vehicles easy
    public abstract class Vehicle
    {
        public string LicensePlate { get; protected set; }
        public VehicleSector Sector { get; protected set; }
        public bool IsTollFree { get; private set; }

        public Vehicle(string licensePlate, VehicleSector sector) 
        {
            LicensePlate = licensePlate;
            Sector = sector;

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
