public class TollCalc
{
    // Kanske lite overkill med en service via DI, men ville ändå visa principen
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
        // Tänkte jag gör övertydliga och mer läsbara if-satser, brukar vara bättre i längden när nya utvecklare ska läsa koden senare.
        if (request == null || request.Vehicle == null)
        {
            return 0;
        }

        if (request.Passages == null || request.Passages.Length == 0)
        {
            return 0;
        }

        if (_tollCalculatorService.IsTollFree(request.Vehicle))
        {
            return 0;
        }

        // Sortera enligt datum, appen hanterar ju bara om det är samma datum just nu.
        var passages = request.Passages.OrderBy(p => p).ToArray();

        // Hämta första datumet som en start, så man slipper hantera detta scenario i loopen senare. 
        DateTime intervalStart = passages[0];
        int intervalFee = GetFeeByTime(intervalStart);
        Console.WriteLine("Före loopen, intervalFee="+intervalFee);

        int totalFee = intervalFee;

        // Då blir loopen lite cleanare och skulle kunna brytas ut till egen metod. 
        foreach (var passage in passages.Skip(1))
        {
            int nextFee = GetFeeByTime(passage);

            // TODO - Otydligt krav om det var per hela timmar, t.ex mellan 06-07 eller 
            // timmen från första passering och en timme framåt, måste utredas.
            bool sameHour =
                passage.Date == intervalStart.Date &&
                passage.Hour == intervalStart.Hour;

            // Endast högsta inom samma timme räknas
            if (sameHour)
            {
                // Är denna passage kostnad större än den vi redan har sparat, korrigera isåfall
                // Annars behöver vi inte göra något, då gäller de dyraste redan.
                // t.ex så är de dyrare efter 06:30 än 06:15
                if (nextFee > intervalFee)
                {
                    // Ta bort fee från föregående, t.ex 8kr
                    totalFee -= intervalFee;
                    // lägg till nya fee, t.ex 13kr
                    totalFee += nextFee;

                    // Spara kostnaden t.ex 13kr inför nästa loop om de skulle vara inom samma timme igen
                    intervalFee = nextFee;
                }
            }
            else
            {
                // Annars lägger vi till passagekostnaden direkt i totalen
                totalFee += nextFee;
                // Korrigerar start till denna timme eftersom de startats en ny timme nu
                intervalStart = passage;
                // Samma som innan, spara kostnaden t.ex 13kr inför nästa loop om de skulle vara inom samma timme igen
                intervalFee = nextFee;
            }

            // Skulle vi få många passager och redan uppfyllt vår maxgräns behöver vi inte loopa mer.
            // OBS - dumt att hårdkoda 60kr i production kod, borde vara config.
            if (totalFee >= 60)
            {
                return 60;
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
        // Letar upp intervalet som passar in på tiden, t.ex 06:15 hittar interval mellan 06:00 - 06:30
        var interval = TollIntervalHelper.TollIntervals.FirstOrDefault(i =>
            time >= i.Start &&
            time < i.End);

        return interval?.Fee ?? 0;;
    }
}