namespace TollFeeCalculator.Core
{
    public interface ITollExemptionRulesService
    {
        bool IsTollFreeVehicle(IVehicle vehicle);
        bool IsTollFreeDate(DateTime date);
    }
}
