namespace TollFeeCalculator
{
    public class Diplomat : IVehicle
    {
        public bool IsTollable { get => false; }

        public override string ToString()
        {
            return "Diplomat";
        }
    }
}