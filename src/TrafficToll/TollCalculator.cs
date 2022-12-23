using TrafficToll.Internals.DataAccess;
using TrafficToll.Internals.Enums;
using TrafficToll.Internals.Services;
using TrafficToll.Internals.Validators;

namespace TrafficToll
{
    public class TollCalculator
    {
        private TollCalculatorCore? _tollCalculatorCore;
        private TollPassingVerifyer? _passingVerifyer;
        private TollPassingVerifyer TollPassingVerifyer => _passingVerifyer 
            ??= new TollPassingVerifyer(TrafficTollDataManager.GetTollFreeParameters());
        private TollCalculatorCore TollCalculatorCore => _tollCalculatorCore 
            ??= new TollCalculatorCore(TrafficTollDataManager.GetTollCalculationParameters());
        public int GetTollFee(IEnumerable<DateTime> passings, int vehicleType)
        {
            ValidatorCalculationArguments.EnsureArgumentsIsValid(passings, vehicleType);
            var tollablePassings = TollPassingVerifyer.GetTollablePassings(passings, (VehicleType)vehicleType);
            if (!tollablePassings.Any()) return 0;
            return TollCalculatorCore.CalculateTollFee(tollablePassings);
        }
    }
}
