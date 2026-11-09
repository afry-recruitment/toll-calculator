namespace TollFeeCalculator;

/// <summary>
/// Supplies the city's approved exemption calendar. Implementations must reject
/// dates outside their configured coverage instead of assuming a chargeable day.
/// </summary>
public interface ITollCalendar
{
    bool IsTollFree(DateOnly date);
}
