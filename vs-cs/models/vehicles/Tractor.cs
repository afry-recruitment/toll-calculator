namespace TollFeeCalculator
{
    public class Tractor : IVehicle
    {
        public bool IsTollable { get => false; }

        public override string ToString()
        {
            return "Tractor";
        }
    }
}