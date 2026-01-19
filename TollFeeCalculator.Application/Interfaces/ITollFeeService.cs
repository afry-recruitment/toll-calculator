using TollFeeCalculator.Application.Models;
using TollFeeCalculator.Enterprise.Entities;

namespace TollFeeCalculator.Application.Interfaces
{
    public interface ITollFeeService
    {
        Task<GetTollFeeResultModel> GetFee(IReadOnlyList<DateTime> dateTimes, Vehicle vehicle, CancellationToken ct);
    }
}
