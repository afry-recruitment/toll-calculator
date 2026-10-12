namespace TollFeeCalculator.Core.Vehicles
{
    /// <summary>
    /// An ordinary passenger car: it always pays toll.
    /// </summary>
    public class Car : IVehicle
    {
        public bool IsTollFree() => false;
    }
}