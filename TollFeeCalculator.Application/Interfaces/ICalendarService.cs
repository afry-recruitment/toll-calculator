namespace TollFeeCalculator.Application.Interfaces
{
    public interface ICalendarService
    {
        Task<bool> IsTollFreeAsync(DateOnly date, CancellationToken ct);
    }
}
