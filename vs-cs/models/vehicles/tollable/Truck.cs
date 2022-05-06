namespace TollFeeCalculator
{
    public class Truck : IVehicle
    {
        public bool IsTollable { get => true; }

        public override string ToString()
        {
            return "Truck";
        }
    }
}