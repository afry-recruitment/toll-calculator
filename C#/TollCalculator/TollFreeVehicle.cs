namespace TollFeeCalculator;

/// <summary>
/// Generic implementation for fee-exempt vehicle categories such as
/// Tractor, Emergency, Diplomat, Foreign and Military, which do not
/// warrant their own concrete class in this kata.
/// </summary>
public class TollFreeVehicle : Vehicle
{
    private readonly string type;

    public TollFreeVehicle(string type)
    {
        this.type = type;
    }

    public string GetVehicleType() => type;
}
