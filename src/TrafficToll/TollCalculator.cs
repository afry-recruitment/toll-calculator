using TrafficToll.Internals;
using TrafficToll.Internals.DataAccess;
using TrafficToll.Internals.Enums;
using TrafficToll.Internals.Validators;

namespace TrafficToll
{
    public class TollCalculator
    {
        private TollCalculatorCore? __tollCalculatorCore;
        private TollCalculatorCore TollCalculatorCore => 
            __tollCalculatorCore ??= new TollCalculatorCore(TrafficTollDataManager.GetTollCalculationParameters());
        public int GetTollFee(IEnumerable<DateTime> passings, int vehicleType)
        {
            ValidatorCalculationArguments.EnsureArgumentsIsValid(passings, vehicleType);
            if (VehicleIsNotTollable(vehicleType))
                return 0;

            return TollCalculatorCore.CalculateTollFee(passings);
        }

        private bool VehicleIsNotTollable(int vehicleType)
        {
            return TrafficTollDataManager.GetTollFreeVehicles().Contains((VehicleType)vehicleType);
        }
    }
}
