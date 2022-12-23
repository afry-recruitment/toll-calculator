using TrafficToll.Internals.DataAccess;
using TrafficToll.Internals.Enums;
using TrafficToll.Internals.Services;
using TrafficToll.Internals.Validators;

namespace TrafficToll
{
    public class TollCalculator
    {
        private TollCalculatorCore? _tollCalculatorCore;
        private PassingVerifyer? _passingVerifyer;
        private PassingVerifyer PassingVerifyer => _passingVerifyer ??= new PassingVerifyer(TrafficTollDataManager.GetTollFreeParameters());
        private TollCalculatorCore TollCalculatorCore => 
            _tollCalculatorCore ??= new TollCalculatorCore(
                TrafficTollDataManager.GetTollCalculationParameters(),
                TrafficTollDataManager.GetTollFreeParameters());
        public int GetTollFee(IEnumerable<DateTime> passings, int vehicleType)
        {
            ValidatorCalculationArguments.EnsureArgumentsIsValid(passings, vehicleType);
            var tollablePassings = PassingVerifyer.GetTollablePassings(passings, (VehicleType)vehicleType);
            if (!tollablePassings.Any()) return 0;
            return TollCalculatorCore.CalculateTollFee(tollablePassings, vehicleType);
        }
    }
}
