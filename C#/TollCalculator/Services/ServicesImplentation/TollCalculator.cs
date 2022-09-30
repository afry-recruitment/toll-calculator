namespace TollFeeCalculator;
public class TollCalculator : ITollCalculator
{
    private readonly DateOnly[] _holidays;

    /// <summary>
    /// TollCalculator with a array of holidays that are exempt from paying toll.
    /// </summary>
    /// <param name="holidays">Array of holiday dates that are exempt from paying toll.</param>
    public TollCalculator(DateOnly[] holidays)
    {
        _holidays = holidays ?? new DateOnly[0];
    }

    /// <summary>
    /// Create a new TollCalculator without specifying holidays that are exempt from toll.
    /// </summary>
    public TollCalculator()
    : this(new DateOnly[0])
    {
    }

    /// <summary>
    /// Calculate the total toll fee for a day.
    /// </summary>
    /// <returns>The total toll fee for that day</returns>
    /// <param name="vehicle">The vehicle</param>
    /// <param name="dates">The date</param>
    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        try
        {
            if (TollFreeHelper.IsTollFreeDate(dates[0], _holidays) ||
            TollFreeHelper.IsTollFreeVehicle(vehicle)) return 0;

            var totalCharges = 0;
            var currentCharges = 0;
            var date = DateOnly.FromDateTime(dates[0]);
            var sortedTimes = dates.Select(dateTime =>
            TimeOnly.FromDateTime(dateTime)).OrderBy(time => time).ToArray();
            var startTime = sortedTimes[0];

            foreach (TimeOnly time in sortedTimes)
            {
                var ChargesPerPass = TollFeeCalculatorHelper.TollChargesPerPass(time.Hour, time.Minute);
                var elapsed = time - startTime;
                if (elapsed > TimeSpan.FromHours(1))
                {
                    totalCharges += currentCharges;
                    startTime = time;
                    currentCharges = ChargesPerPass;
                }
                else
                {
                    currentCharges = Math.Max(ChargesPerPass, currentCharges);
                }
            }
            totalCharges += currentCharges;
            return Math.Min(totalCharges, 60);
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message.ToString());
        }
        catch (Exception)
        {
            throw new Exception("Error encountered, please contact the admin");
        }
    }
}