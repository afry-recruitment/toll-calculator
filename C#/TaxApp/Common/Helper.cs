namespace TaxApp;

public static class Helper
{
    public static List<DateTime> DatesWithoutCharge
    {
        get
        {
            return new List<DateTime>
            {
                new DateTime(2025, 1, 1),
                new DateTime(2025, 1, 6),
                new DateTime(2025, 4, 17),
                new DateTime(2025, 4, 18),
                new DateTime(2025, 4, 30),
                new DateTime(2025, 5, 1),
                new DateTime(2025, 5, 28),
                new DateTime(2025, 5, 29),
                new DateTime(2025, 6, 5),
                new DateTime(2025, 6, 6),
                new DateTime(2025, 6, 20),

                new DateTime(2025, 10, 31),
                new DateTime(2025, 12, 24),
                new DateTime(2025, 12, 25),
                new DateTime(2025, 12, 26),
                new DateTime(2025, 12, 31)
            };
        }
    }

}
