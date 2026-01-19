namespace TollFeeCalculator.Infrastructure.Policies.Holidays
{
    public static class Holidays
    {
        public static IReadOnlySet<DateOnly> ByYear(int year) => Sweden.Where(d => d.Year == year).ToHashSet();

        public static readonly IReadOnlySet<DateOnly> Sweden = new HashSet<DateOnly>
        {
            new(2026, 1, 1),   // Nyarsdagen
            new(2026, 1, 6),   // Trettondedag jul
            new(2026, 4, 3),   // Langfredagen
            new(2026, 4, 5),   // Paskdagen (Sunday)
            new(2026, 4, 6),   // Annandag pask
            new(2026, 5, 1),   // Forsta maj
            new(2026, 5, 14),  // Kristi himmelfardsdag
            new(2026, 5, 24),  // Pingstdagen
            new(2026, 6, 6),   // Sveriges nationaldag
            new(2026, 6, 20),  // Midsommardagen
            new(2026, 10, 31), // Alla helgons dag
            new(2026, 12, 25), // Juldagen
            new(2026, 12, 26), // Annandag jul
        };
    }
}
