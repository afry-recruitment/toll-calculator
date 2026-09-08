namespace TollFeeCalculatorLibrary;

public enum TollFreeVehicles
{
    Motorbike = 0,
    Tractor = 1,
    Emergency = 2,
    Diplomat = 3,
    Foreign = 4,
    Military = 5


}

static class DurationExtensions
{
    static readonly string[] TollFreeVehicleStrings = [.. Enum
            .GetValues<TollFreeVehicles>()
            .Select(x => x.ToString())];

    extension(IVehicle vehicle)
    {
        public bool IsTollFreeVehicle()
        {
            return TollFreeVehicleStrings.Contains(vehicle.GetVehicleType());
        }
    }
}