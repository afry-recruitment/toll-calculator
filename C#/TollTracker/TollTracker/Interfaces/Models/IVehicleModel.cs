using TollTracker.Enums;

namespace TollTracker.Interfaces.Models;

/// <summary>
/// Provides properties for a vehicle model.
/// </summary>
public interface IVehicleModel
{
    /// <summary>
    /// Gets the registrationNumber associated with the vehicle.
    /// </summary>
    string RegistrationNumber { get; }

    /// <summary>
    /// Gets the type of the vehicle.
    /// </summary>
    VehicleType Type { get; }

    /// <summary>
    /// Gets the model of the vehicle.
    /// </summary>
    string Model { get; }

    /// <summary>
    /// Gets the name of owner associated with the vehicle. 
    /// </summary>
    string Owner { get; }
}

