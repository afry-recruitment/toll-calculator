using TrafficToll.Internals;
using TrafficToll.Internals.DataAccess;
using TrafficToll.Internals.Validators;

namespace TrafficToll
{
    public class TollCalculator
    {
        public static int GetTollFee(IEnumerable<DateTime> passings, int vehicleType)
        {
            ValidatorCalculationArguments.EnsureArgumentsIsValid(passings, vehicleType);

            var tollCalculationInput = TrafficTollFactory.CreateTollCalculationInput(passings, vehicleType);
            var trafficTollSpecification = TrafficTollSpecificationRepository.GetDailyTrafficTollSpecification();
            var tollCalculationArguments = TrafficTollFactory.CreateTollCalculationArguments(trafficTollSpecification);

            throw new NotImplementedException();
            //return TollCalculatorCore.GetTollFee(tollCalculationInput, tollCalculationArguments);
        }
    }
}
