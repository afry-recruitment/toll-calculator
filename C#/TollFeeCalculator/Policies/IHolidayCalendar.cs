namespace TollFeeCalculator.Policies;

public interface IHolidayCalendar
{
    bool IsTollFree(DateTime date);
}
