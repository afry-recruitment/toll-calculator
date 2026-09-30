namespace TollFeeCalculator;

/// <summary>A vehicle classified using the types supported by the toll rules.</summary>
public sealed class Vehicle
{
    public Vehicle(VehicleType type)
    {
        if (!Enum.IsDefined(type))
        {
            throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown vehicle type.");
        }

        Type = type;
    }

    public VehicleType Type { get; }
}
