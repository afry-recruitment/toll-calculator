using TollFeeCalculator;
using TollFeeCalculator.Policies;
using TollFeeCalculator.Services;

public class TollCalculator
{

    /**
    * Calculate the total toll fee for one day
    *
    * @param vehicle - the vehicle
    * @param dates   - date and time of all passes on one day
    * @return - the total toll fee for that day
    */
    private readonly TollFeeService tollFeeService;

    public TollCalculator()
        : this(new TollFeeService(
            new TollFeeSchedule(),
            new TollFreeVehiclePolicy(),
            new HolidayCalendar()))
    {
    }

    public TollCalculator(TollFeeService tollFeeService)
    {
        this.tollFeeService = tollFeeService ?? throw new ArgumentNullException(nameof(tollFeeService));
    }

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        return tollFeeService.Calculate(vehicle, dates);
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        return tollFeeService.GetTollFee(date, vehicle);
    }
}
