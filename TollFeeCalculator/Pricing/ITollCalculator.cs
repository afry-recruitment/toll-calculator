using TollFeeCalculator.Core;

namespace TollFeeCalculator.Pricing
{
    public interface ITollCalculator
    {
        decimal CalculateToll(IVehicle vehicle, DateTime time);
    }
}
