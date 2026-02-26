namespace TollFeeCalculator.Interfaces;

public interface IHolidayProvider
{
    bool IsHoliday(DateTime date);
}