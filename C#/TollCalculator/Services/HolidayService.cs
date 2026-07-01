using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using PublicHoliday;

namespace TollCalculator.Services;

/// <summary>
/// Determines whether a given date is exempt from toll fees.
/// Handles weekends, Swedish public holidays, days before holidays, and July.
/// Uses PublicHoliday library for accurate holiday data across all years.
/// </summary>
public class HolidayService
{
    private readonly SwedenPublicHoliday _calendar = new();
    private readonly ILogger<HolidayService> _logger;

    public HolidayService(ILogger<HolidayService>? logger = null)
    {
        _logger = logger ?? NullLogger<HolidayService>.Instance;
    }

    /// <summary>
    /// Returns true if the given date is exempt from toll fees.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns>True if the date is toll-free, otherwise false.</returns>
    /// <exception cref="ArgumentException">Thrown if date is a default value.</exception>
    public bool IsTollFreeDate(DateTime date)
    {
        if (date == default)
            throw new ArgumentException("Date cannot be a default value.", nameof(date));

        if (IsWeekend(date))
        {
            _logger.LogDebug("Date {Date} is a weekend. Toll-free.", date.ToShortDateString());
            return true;
        }

        if (IsJuly(date))
        {
            _logger.LogDebug("Date {Date} is in July. Toll-free.", date.ToShortDateString());
            return true;
        }

        if (IsPublicHoliday(date))
        {
            _logger.LogDebug("Date {Date} is a public holiday. Toll-free.", date.ToShortDateString());
            return true;
        }

        if (IsDayBeforePublicHoliday(date))
        {
            _logger.LogDebug("Date {Date} is the day before a public holiday. Toll-free.", date.ToShortDateString());
            return true;
        }

        return false;
    }

    private static bool IsWeekend(DateTime date)
        => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

    private static bool IsJuly(DateTime date)
        => date.Month == 7;

    private bool IsPublicHoliday(DateTime date)
        => _calendar.IsPublicHoliday(date);

    private bool IsDayBeforePublicHoliday(DateTime date)
    {
        if (date.Date == DateTime.MaxValue.Date) return false;
        return _calendar.IsPublicHoliday(date.AddDays(1));
    }
}