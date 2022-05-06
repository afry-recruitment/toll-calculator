namespace TollFeeCalculator
{
    public class Emergency : IVehicle
    {
        public bool IsTollable { get => false; }

        public override string ToString()
        {
            return "Emergency";
        }
    }
}