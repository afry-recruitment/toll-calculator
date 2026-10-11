using System;

namespace TollFeeCalculator.Tests;

/// <summary>
/// Shared fixtures. 2013-01-08 is a Tuesday, so it is a normal chargeable
/// weekday; 2013-01-05 is a Saturday.
/// </summary>
internal static class Days
{
    public static readonly DateTime Tuesday = new(2013, 1, 8);
    public static readonly DateTime Saturday = new(2013, 1, 5);
    public static readonly DateTime Sunday = new(2013, 1, 6);

    /// <summary>A time on the normal Tuesday.</summary>
    public static DateTime Tue(int hour, int minute = 0) => Tuesday.AddHours(hour).AddMinutes(minute);

    /// <summary>A time on an arbitrary day.</summary>
    public static DateTime On(DateTime day, int hour, int minute = 0) => day.Date.AddHours(hour).AddMinutes(minute);
}

/// <summary>
/// A <see cref="Vehicle"/> whose type is whatever name it is constructed with,
/// used for the toll-free types that have no dedicated class.
/// </summary>
internal sealed class NamedVehicle : Vehicle
{
    private readonly string _type;

    public NamedVehicle(string type) => _type = type;

    public string GetVehicleType() => _type;
}
