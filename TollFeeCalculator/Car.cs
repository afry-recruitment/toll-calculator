namespace TollFeeCalculator
{
    public class Car : IVehicle
    {
        private bool _tollFree = false;

        public string VehicleType => "Car";
        public bool IsTollFreeVehicle
        {
            get => _tollFree;
            set => _tollFree = value;
        }
    }
}