public class TollCalc
{
    private record TollInterval(
        TimeSpan Start,
        TimeSpan End,
        int Fee);

    private static readonly List<TollInterval> TollIntervals =
    [
        new(new TimeSpan(6, 0, 0),  new TimeSpan(6, 30, 0), 8),
        new(new TimeSpan(6, 30, 0), new TimeSpan(7, 0, 0), 13),
        new(new TimeSpan(7, 0, 0),  new TimeSpan(8, 0, 0), 18),
        new(new TimeSpan(8, 0, 0),  new TimeSpan(8, 30, 0), 13),
        new(new TimeSpan(8, 30, 0), new TimeSpan(15, 0, 0), 8),
        new(new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0), 13),
        new(new TimeSpan(15, 30, 0), new TimeSpan(17, 0, 0), 18),
        new(new TimeSpan(17, 0, 0), new TimeSpan(18, 0, 0), 13),
        new(new TimeSpan(18, 0, 0), new TimeSpan(18, 30, 0), 8),
    ];

    private readonly ITollCalculatorService _tollCalculatorService;

    public TollCalc(ITollCalculatorService tollCalculatorService)
    {
        _tollCalculatorService = tollCalculatorService;
    }

    /**
     * Calculate the total toll fee for one day based on passages
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
public int GetTollFeeByDayPassages(TollFeeRequest request)
{
    if (request == null || request.Vehicle == null)
    {
        Console.WriteLine("Första");
        return 0;
    }

    if (request.Passages == null || request.Passages.Length == 0)
    {
        Console.WriteLine("Andra");
        return 0;
    }

    if (_tollCalculatorService.IsTollFree(request.Vehicle))
    {
        Console.WriteLine("Tredje");
        return 0;
    }

    var passages = request.Passages.OrderBy(p => p).ToArray();

    DateTime intervalStart = passages[0];
    int intervalFee = GetFeeByTime(intervalStart);
    Console.WriteLine("intervalFee="+intervalFee);

    int totalFee = intervalFee;

    foreach (var passage in passages.Skip(1))
    {
        int nextFee = GetFeeByTime(passage);

        bool sameHour =
            passage.Date == intervalStart.Date &&
            passage.Hour == intervalStart.Hour;

        if (sameHour)
        {
            if (nextFee > intervalFee)
            {
                totalFee -= intervalFee;
                totalFee += nextFee;

                intervalFee = nextFee;
            }
        }
        else
        {
            totalFee += nextFee;

            intervalStart = passage;
            intervalFee = nextFee;
        }
    }

    return Math.Min(totalFee, 60);
}

    private int GetFeeByTime(DateTime passage)
    {
        if (TollDateHelper.IsTollFreeDate(passage))
        {
            return 0;
        }

        var time = passage.TimeOfDay;

        var interval = TollIntervals.FirstOrDefault(i =>
            time >= i.Start &&
            time < i.End);

        return interval?.Fee ?? 0;
    }
}