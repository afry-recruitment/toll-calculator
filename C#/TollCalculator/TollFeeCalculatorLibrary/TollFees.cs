namespace TollFeeCalculatorLibrary;

internal class TollFees
{
    readonly TollFreeDates tollFreeDates;

    readonly int[] tollFee = new int[1440]; // There are 1440 minutes in a day.

    public TollFees()
    {
        tollFreeDates = new();

        /* For the fun of it we use a LUT for toll fees.
             * Could even be a thing in some high perf scenarios.
             */
        for (int m = 0; m < 1440; m++)
            tollFee[m] = TollFee(m);

        static int TollFee(int minutesFromMidnight)
        {
            return minutesFromMidnight switch
            {
                >= 390 and < 420 => 13, // 06:30 - 06:59
                >= 420 and < 480 => 18, // 07:00 - 07:59
                >= 480 and < 510 => 13, // 08:00 - 08:29
                >= 900 and < 930 => 13, // 15:00 - 15:29
                >= 930 and < 1020 => 18, // 15:30 - 16:59
                >= 1020 and < 1080 => 13, // 17:00 - 17:59
                _ => 8
            };
        }
    }

    public int FeeForDate(DateTime date)
    {
        DateOnly dateOnly = DateOnly.FromDateTime(date);

        if (tollFreeDates.IsTollFreeDate(dateOnly)) return 0;

        // Convert everything to total minutes from midnight
        int m = (date.Hour * 60) + date.Minute;

        return tollFee[m];
    }
}
