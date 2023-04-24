namespace TollCalculator.Models
{
    // Made an abstract class instead of an interface to increase flexibility and makes adding new vehicles easy
    public abstract class Vehicle
    {
        public string LicensePlate { get; protected set; }
        public bool IsTollFree { get; private set; }

        public Vehicle(string licensePlate) 
        {
            LicensePlate = licensePlate;

            IsTollFree = Enum.IsDefined(typeof(TollFreeVehicle), GetType().Name);
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
