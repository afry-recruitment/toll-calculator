namespace toll_calculator_jakob.Data
{
    public static class Holidays
    {
        // Days before public holidays are not present in this list
        // Need to be added
        // 3rd party API was used to fetch these dates

        public static readonly Dictionary<int, HashSet<DateOnly>> Data = new()
        {
            { 2024,
            [
                new (2024, 1, 1),
                new (2024, 1, 6),
                new (2024, 3, 29),
                new (2024, 3, 31),
                new (2024, 4, 1),
                new (2024, 5, 1),
                new (2024, 5, 9),
                new (2024, 5, 19),
                new (2024, 6, 6),
                new (2024, 6, 21),
                new (2024, 6, 22),
                new (2024, 11, 2),
                new (2024, 12, 24),
                new (2024, 12, 25),
                new (2024, 12, 26),
                new (2024, 12, 31)
            ]
            },
            { 2025,
            [
                new (2025, 1, 1),
                new (2025, 1, 6),
                new (2025, 4, 18),
                new (2025, 4, 20),
                new (2025, 4, 21),
                new (2025, 5, 1),
                new (2025, 5, 29),
                new (2025, 6, 6),
                new (2025, 6, 8),
                new (2025, 6, 20),
                new (2025, 6, 21),
                new (2025, 11, 1),
                new (2025, 12, 24),
                new (2025, 12, 25),
                new (2025, 12, 26),
                new (2025, 12, 31)
            ]
            },
            { 2026,
            [
                new (2026, 1, 1),
                new (2026, 1, 6),
                new (2026, 4, 3),
                new (2026, 4, 5),
                new (2026, 4, 6),
                new (2026, 5, 1),
                new (2026, 5, 14),
                new (2026, 5, 24),
                new (2026, 6, 6),
                new (2026, 6, 19),
                new (2026, 6, 20),
                new (2026, 10, 31),
                new (2026, 12, 24),
                new (2026, 12, 25),
                new (2026, 12, 26),
                new (2026, 12, 31)
            ]
            }
        };
    }
}
