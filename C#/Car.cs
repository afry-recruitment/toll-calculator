namespace TollFeeCalculator
{
    public class Car : Vehicle
    {
        public Car(string licensePlate) : base(licensePlate)
        {
        }

        public override string GetVehicleType()
        {
            return "Car";
        }
    }
}