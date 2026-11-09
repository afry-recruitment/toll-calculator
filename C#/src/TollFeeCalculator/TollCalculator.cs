namespace TollFeeCalculator;

/// <summary>Calculates toll fees for one vehicle and one city-local calendar day.</summary>
public sealed class TollCalculator
{
    public const int MaximumDailyFee = 60;
    private static readonly TimeSpan ChargingWindow = TimeSpan.FromMinutes(60);
    private readonly ITollCalendar calendar;

    public TollCalculator() : this(ConfiguredTollCalendar.For2013()) { }

    public TollCalculator(ITollCalendar calendar)
    {
        ArgumentNullException.ThrowIfNull(calendar);
        this.calendar = calendar;
    }

    /// <summary>
    /// Returns the daily total in whole SEK. Supply the complete day's passages
    /// as city-local DateTimes with Kind == Unspecified. Input order and duplicates
    /// do not matter; the caller's collection is never changed. Empty input costs zero.
    /// </summary>
    public int CalculateDailyFee(VehicleType vehicle, IEnumerable<DateTime> passages)
    {
        bool exemptVehicle = IsExemptVehicle(vehicle);
        ArgumentNullException.ThrowIfNull(passages);

        // Materialize once and sort our own copy, including when input is an array.
        DateTime[] orderedPassages = passages.ToArray();
        if (orderedPassages.Length == 0) return 0;

        DateOnly day = DateOnly.FromDateTime(orderedPassages[0]);
        foreach (DateTime passage in orderedPassages)
        {
            ValidateTimestamp(passage);
            if (DateOnly.FromDateTime(passage) != day)
                throw new ArgumentException("All passages must belong to one local calendar day.", nameof(passages));
        }

        // Invalid dates must not be hidden by a vehicle or calendar exemption.
        bool exemptDate = calendar.IsTollFree(day);
        if (exemptVehicle || exemptDate) return 0;

        Array.Sort(orderedPassages);
        DateTime? windowStart = null;
        int windowMaximum = 0;
        int total = 0;

        foreach (DateTime passage in orderedPassages)
        {
            int fee = TollTariff.GetFee(TimeOnly.FromDateTime(passage));
            if (fee == 0) continue; // Free passages never start, move or reset a window.

            if (windowStart is null || passage - windowStart.Value > ChargingWindow)
            {
                // Close the previous window, then anchor a new one to this passage.
                total += windowMaximum;
                windowStart = passage;
                windowMaximum = fee;
            }
            else
            {
                // Exactly 60 minutes is included. A higher fee does not move the anchor.
                windowMaximum = Math.Max(windowMaximum, fee);
            }
        }

        // The last window has no later passage to close it.
        return Math.Min(total + windowMaximum, MaximumDailyFee);
    }

    /// <summary>Returns one passage's fee before hourly grouping and the daily cap.</summary>
    public int GetPassageFee(VehicleType vehicle, DateTime passage)
    {
        bool exemptVehicle = IsExemptVehicle(vehicle);
        ValidateTimestamp(passage);
        bool exemptDate = calendar.IsTollFree(DateOnly.FromDateTime(passage));

        return exemptVehicle || exemptDate ? 0 : TollTariff.GetFee(TimeOnly.FromDateTime(passage));
    }

    private static bool IsExemptVehicle(VehicleType vehicle) => vehicle switch
    {
        VehicleType.Car or VehicleType.Other => false,
        VehicleType.Motorbike or VehicleType.Tractor or VehicleType.Emergency
            or VehicleType.Diplomat or VehicleType.Foreign or VehicleType.Military => true,
        _ => throw new ArgumentOutOfRangeException(nameof(vehicle), vehicle, "Choose a recognized vehicle category.")
    };

    private static void ValidateTimestamp(DateTime passage)
    {
        if (passage.Kind != DateTimeKind.Unspecified)
            throw new ArgumentException(
                "Use city-local time with DateTimeKind.Unspecified. Convert UTC at the application boundary.",
                nameof(passage));
    }
}
