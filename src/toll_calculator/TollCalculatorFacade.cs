using toll_calculator.value_objects;

namespace toll_calculator
{
    public class TollCalculatorFacade
    {
        public static int GetTollFee(IEnumerable<DateTime> passings, int vehicleType)
        {
            return TollCalculator.GetTollFee(new TollCalculationInput(passings, vehicleType));
        }
    }
}
