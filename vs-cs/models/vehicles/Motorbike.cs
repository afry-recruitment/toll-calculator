namespace TollFeeCalculator
{
    public class Motorbike : IVehicle
    {
        public bool IsTollable { get => false; }

        public override string ToString()
        {
            return "Motorbike";
        }
    }
}
