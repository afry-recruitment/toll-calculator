using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using TollCalculator.Models;
using TollCalculator.Services;
using TollCalculator.Settings;

namespace TollCalculator;

/// <summary>
/// Calculates toll fees for vehicles passing through a toll station.
/// 
/// Charging rules:
/// - Fees vary between 8 and 18 SEK depending on time of day.
/// - A vehicle is only charged once per hour. If multiple passes occur within
///   the same 60-minute window, only the highest fee applies.
/// - The maximum fee per day is 60 SEK.
/// - Toll-free vehicle types are never charged.
/// - Weekends, public holidays, the day before a public holiday, and July are toll-free.
/// 
/// Business rules are configurable via <see cref="TollSettings"/>.
/// </summary>
public class TollCalculator
{
    private readonly HolidayService _holidayService;
    private readonly TollSettings _settings;
    private readonly ILogger<TollCalculator> _logger;

    public TollCalculator(
        HolidayService? holidayService = null,
        TollSettings? settings = null,
        ILogger<TollCalculator>? logger = null)
    {
        _holidayService = holidayService ?? new HolidayService();
        _settings = settings ?? new TollSettings();
        _logger = logger ?? NullLogger<TollCalculator>.Instance;
    }

    /// <summary>
    /// Calculate the total toll fee for one day.
    /// </summary>
    /// <param name="vehicle">The vehicle passing through.</param>
    /// <param name="dates">All passage timestamps for a single day.</param>
    /// <returns>Total toll fee in SEK, capped at MaxDailyFee.</returns>
    /// <exception cref="ArgumentNullException">Thrown if vehicle or dates is null.</exception>
    /// <exception cref="ArgumentException">Thrown if dates is empty.</exception>
    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
        if (dates == null) throw new ArgumentNullException(nameof(dates));
        if (dates.Length == 0) throw new ArgumentException("Dates cannot be empty.", nameof(dates));

        _logger.LogInformation("Calculating toll fee for vehicle type {VehicleType} with {PassCount} passes.",
            vehicle.GetVehicleType(), dates.Length);

        if (vehicle.GetVehicleType().IsTollFree())
        {
            _logger.LogInformation("Vehicle type {VehicleType} is toll-free. Returning 0.",
                vehicle.GetVehicleType());
            return 0;
        }

        var passes = dates
            .Where(d => !_holidayService.IsTollFreeDate(d))
            .OrderBy(d => d)
            .ToList();

        if (passes.Count == 0)
        {
            _logger.LogInformation("All passes are on toll-free dates. Returning 0.");
            return 0;
        }

        int totalFee = 0;
        DateTime windowStart = passes[0];
        int windowMax = GetFeeForTime(passes[0]);

        foreach (var pass in passes.Skip(1))
        {
            double minutes = (pass - windowStart).TotalMinutes;
            int fee = GetFeeForTime(pass);

            if (minutes < _settings.HourWindowMinutes)
            {
                windowMax = Math.Max(windowMax, fee);
            }
            else
            {
                _logger.LogDebug("New hour window started at {Pass}. Committing fee {Fee} SEK.",
                    pass, windowMax);
                totalFee += windowMax;
                windowStart = pass;
                windowMax = fee;
            }
        }

        totalFee += windowMax;
        int finalFee = Math.Min(totalFee, _settings.MaxDailyFee);

        _logger.LogInformation("Total toll fee calculated: {Fee} SEK.", finalFee);

        return finalFee;
    }

    /// <summary>
    /// Returns the toll fee for a specific time of day based on the configured fee schedule.
    /// </summary>
    /// <param name="date">The datetime to check.</param>
    /// <returns>The fee in SEK, or 0 if outside all intervals.</returns>
    public int GetFeeForTime(DateTime date)
    {
        var time = TimeOnly.FromDateTime(date);
        return _settings.FeeSchedule.FirstOrDefault(i => i.Contains(time))?.Fee ?? 0;
    }
}