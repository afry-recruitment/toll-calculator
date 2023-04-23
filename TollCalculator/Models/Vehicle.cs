namespace TollCalculator.Models
{
    // I will only create two classes that inherit from Vehicle, Motorbike (toll-free) and Car (not toll-free) to avoid unneccessery classes since anymore are superfluous
    public abstract class Vehicle
    {
        // LicensePlate needs to be unique, but for simplicities sake I ensure that by sending it in as a parameter in the constructor
        public string LicensePlate { get; protected set; }
        public bool IsTollFree { get; protected set; }
    }
}
