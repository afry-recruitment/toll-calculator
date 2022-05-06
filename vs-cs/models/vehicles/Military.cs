namespace TollFeeCalculator
{
    public class Military : IVehicle
    {
        public bool IsTollable { get => false; }

        public override string ToString()
        {
            return "Military";
        }
    }
}