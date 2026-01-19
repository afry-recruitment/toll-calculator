using TollFeeCalculator.Application.Interfaces;
using TollFeeCalculator.Infrastructure.Policies.Holidays;

namespace TollFeeCalculator.Infrastructure.Providers
{
    public class SwedenHolidayProvider : IHolidayProvider
    {
        public async Task<IReadOnlySet<DateOnly>> GetHolidaysAsync(int year, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            return Holidays.ByYear(year);
        }
    }
}
