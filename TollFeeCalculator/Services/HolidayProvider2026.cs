using TollFeeCalculator.Interfaces;

namespace TollFeeCalculator.Services;

/// <summary>
/// Holiday provider for 2026.
/// In production, this could be replaced with:
/// - HolidayProviderApi (fetches from external service)
/// - HolidayProviderDatabase (loads from database)
/// - HolidayProviderConfig (reads from JSON file)
/// </summary>
public class HolidayProvider2026 : IHolidayProvider
{
    public bool IsHoliday(DateTime date)
    {
        return date switch
        {
            // Toll-free: weekends
            { DayOfWeek: DayOfWeek.Saturday or DayOfWeek.Sunday } => true,

            // Toll-free: 2026 holidays
            { Year: 2026, Month: 1, Day: 1 or 6 } => true,
            { Year: 2026, Month: 4, Day: 3 or 5 or 6 } => true,
            { Year: 2026, Month: 5, Day: 1 or 14 or 24 } => true,
            { Year: 2026, Month: 6, Day: 6 or 19 or 20 } => true,
            { Year: 2026, Month: 10, Day: 31 } => true,
            { Year: 2026, Month: 12, Day: 24 or 25 or 26 } => true,
            _ => false
        };
    }
}