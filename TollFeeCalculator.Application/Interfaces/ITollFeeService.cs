using TollFeeCalculator.Domain.Interfaces;

namespace TollFeeCalculator.Application.Interfaces
{
    public interface ITollFeeService
    {
        int CalculateTollFee(IVehicle vehicle, DateTime[] dates);
    }
}
