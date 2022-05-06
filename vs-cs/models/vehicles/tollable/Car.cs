namespace TollFeeCalculator
{
    public class Car : IVehicle
    {
        public bool IsTollable { get => true; }

        public override string ToString()
        {
            return "Car";
        }
    }
}