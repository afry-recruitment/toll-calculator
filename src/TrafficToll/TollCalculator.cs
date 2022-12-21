using TrafficToll.Internals;
using TrafficToll.Internals.DataAccess;

namespace toll_calculator
{
    public class TollCalculator
    {
        public static int GetTollFee(IEnumerable<DateTime> passings, int vehicleType)
        {
            var tollCalculationInput = TrafficTollFactory.CreateTollCalculationInput(passings, vehicleType);
            var trafficTollSpecification = TrafficTollSpecificationRepository.GetDailyTrafficTollSpecification();
            var tollCalculationArguments = TrafficTollFactory.CreateTollCalculationArguments(trafficTollSpecification);

            throw new NotImplementedException();
            //return TollCalculatorCore.GetTollFee(tollCalculationInput, tollCalculationArguments);
        }
    }
}
