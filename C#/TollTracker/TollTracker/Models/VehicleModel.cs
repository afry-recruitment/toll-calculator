using System.Text.Json.Serialization;
using TollTracker.Enums;
using TollTracker.Interfaces.Models;

namespace TollTracker.Models;

public class VehicleModel : IVehicleModel
{
    public string RegistrationNumber { get; init; } = null!;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public VehicleType Type { get; init; }

    public string Owner { get; init; } = null!;

    public string Model {  get; init; } = null!;
}