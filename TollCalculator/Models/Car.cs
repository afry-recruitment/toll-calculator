namespace TollCalculator.Models
{
    public class Car : Vehicle
    {
        public Car(string licensePlate, VehicleSector sector) : base(licensePlate, sector) { }
    }
}