namespace TollCalculator.Models
{
    public class Motorbike : Vehicle
    {
        public Motorbike(string licensePlate)
        {
            LicensePlate = licensePlate;
            IsTollFree = true;
        }
    }
}
