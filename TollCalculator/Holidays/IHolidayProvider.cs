namespace TollCalculator.Holidays;

public interface IHolidayProvider
{
    bool IsHoliday(DateTime date);
}