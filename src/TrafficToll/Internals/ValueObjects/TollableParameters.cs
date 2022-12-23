using TrafficToll.Internals.Enums;

namespace TrafficToll.Internals.ValueObjects;

internal sealed record TollableParameters
{
    public TollableParameters(
        IEnumerable<VehicleType> tollFreeVehicles,
        IEnumerable<(int year, int month, int day)> tollFreeDates)
    {
        TollFreeVehicles = tollFreeVehicles;
        TollFreeDates = tollFreeDates;
    }

    public IEnumerable<VehicleType> TollFreeVehicles { get; }
    public IEnumerable<(int year, int month, int day)> TollFreeDates { get; }
}