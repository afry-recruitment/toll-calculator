namespace TollFeeCalculatorLibrary;

internal class IsTollFreeVehicle
{
    readonly string[] TollFreeVehicleStrings;

    public IsTollFreeVehicle()
    {
        TollFreeVehicleStrings = [.. Enum
            .GetValues<TollFreeVehicles>()
            .Select(x => x.ToString())];
    }
}
