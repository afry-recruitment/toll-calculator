namespace TollFeeCalculator.Application.Interfaces
{
    public interface IHolidayProvider
    {
        Task<IReadOnlySet<DateOnly>> GetHolidaysAsync(int year, CancellationToken ct);
    }
}
