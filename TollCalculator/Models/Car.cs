namespace TollCalculator.Models
{
    public class Car : Vehicle
    {
        public Car(string licensePlate)
        {
            LicensePlate = licensePlate;
            IsTollFree = false;
        }
    }
}