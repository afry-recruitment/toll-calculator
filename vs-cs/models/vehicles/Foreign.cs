namespace TollFeeCalculator
{
    public class Foreign : IVehicle
    {
        public bool IsTollable { get => false; }

        public override string ToString()
        {
            return "Foreign";
        }
    }
}