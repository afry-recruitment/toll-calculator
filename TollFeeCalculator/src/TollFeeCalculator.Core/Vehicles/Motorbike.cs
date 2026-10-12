namespace TollFeeCalculator.Core.Vehicles
{
    /// <summary>
    /// A motorbike: toll-free under the system rules.
    /// The exemption lives here (and not in the calculator) so adding a new exempt
    /// category - Tractor, Emergency, Diplomat... - is just a new class.
    /// </summary>
    public class Motorbike : IVehicle
    {
        public bool IsTollFree() => true;
    }
}
