using TollFeeCalculator.Policies;

namespace TollFeeCalculator.Services;

public class TollFeeService
{
    public const int DailyMaximum = 60;

    private static readonly TimeSpan ChargingWindow = TimeSpan.FromHours(1);
    private readonly ITollFeeSchedule feeSchedule;
    private readonly ITollFreeVehiclePolicy vehiclePolicy;
    private readonly IHolidayCalendar holidayCalendar;

    public TollFeeService(
        ITollFeeSchedule feeSchedule,
        ITollFreeVehiclePolicy vehiclePolicy,
        IHolidayCalendar holidayCalendar)
    {
        this.feeSchedule = feeSchedule ?? throw new ArgumentNullException(nameof(feeSchedule));
        this.vehiclePolicy = vehiclePolicy ?? throw new ArgumentNullException(nameof(vehiclePolicy));
        this.holidayCalendar = holidayCalendar ?? throw new ArgumentNullException(nameof(holidayCalendar));
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (vehiclePolicy.IsTollFree(vehicle) || holidayCalendar.IsTollFree(date))
            return 0;

        return feeSchedule.GetFee(date);
    }

    public int Calculate(Vehicle vehicle, IReadOnlyList<DateTime>? dates)
    {
        if (dates == null || dates.Count == 0 || vehiclePolicy.IsTollFree(vehicle))
            return 0;

        DateTime[] orderedDates = new DateTime[dates.Count];
        for (int index = 0; index < dates.Count; index++)
            orderedDates[index] = dates[index];

        Array.Sort(orderedDates);

        DateTime intervalStart = orderedDates[0];
        int intervalFee = GetTollFee(intervalStart, vehicle);
        int totalFee = 0;

        for (int index = 1; index < orderedDates.Length; index++)
        {
            DateTime date = orderedDates[index];
            if (date - intervalStart <= ChargingWindow)
            {
                intervalFee = Math.Max(intervalFee, GetTollFee(date, vehicle));
                continue;
            }

            totalFee += intervalFee;
            if (totalFee >= DailyMaximum)
                return DailyMaximum;

            intervalStart = date;
            intervalFee = GetTollFee(date, vehicle);
        }

        return Math.Min(DailyMaximum, totalFee + intervalFee);
    }
}
