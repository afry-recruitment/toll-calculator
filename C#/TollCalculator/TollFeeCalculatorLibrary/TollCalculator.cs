namespace TollFeeCalculatorLibrary;

public class TollCalculator
{
    readonly TollFees tollFees = new();

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        /* This precondition is important and should
         * probably be guarded with the type system. */
        if (dates.GroupBy(x => x.Date).Count() > 1)
            throw new Exception("This should be a better exception!");

        if (vehicle.IsTollFreeVehicle()) return 0;

        var allFees = dates
            .Select(date => (date, tollFees.FeeForDate(date)))
            .GroupBy(d => d.date.Hour)
            .Select(x => x.Max(y => y.Item2));

        int maxFee = Math.Min(allFees.Sum(), 60);

        return maxFee;
    }

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (vehicle.IsTollFreeVehicle()) return 0;

        return tollFees.FeeForDate(date);
    }

    public enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}