using TollFeeCalculator.Domain.Interfaces;

namespace TollFeeCalculator.Infrastructure.Interfaces
{
    public interface ITollRepository
    {
        bool IsTollFreeVehicle(IVehicle vehicle);
        bool IsTollFreeDate(DateTime date);
        int GetTollFeeByTime(DateTime date);
    }
}
