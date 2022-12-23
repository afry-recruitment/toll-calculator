using TrafficToll.Internals.DataAccess;
using TrafficToll.Internals.Enums;
using TrafficToll.Internals.Services;
using TrafficToll.Internals.Validators;

namespace TrafficToll
{
    public class TollCalculator
    {
        private TollCalculatorCore? __tollCalculatorCore;
        private TollCalculatorCore TollCalculatorCore => 
            __tollCalculatorCore ??= new TollCalculatorCore(
                TrafficTollDataManager.GetTollCalculationParameters(),
                TrafficTollDataManager.GetTollFreeParameters());
        public int GetTollFee(IEnumerable<DateTime> passings, int vehicleType)
        {
            ValidatorCalculationArguments.EnsureArgumentsIsValid(passings, vehicleType);
            return TollCalculatorCore.CalculateTollFee(passings, vehicleType);
        }
    }
}
