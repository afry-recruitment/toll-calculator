package time

/**
 * Time ranges
 * 00:00 - 05:59 = 0
 * 06:00 - 06:29 = 8
 * 06:30 - 06:59 = 13
 * 07:00 - 07:59 = 18
 * 08:00 - 08:29 = 13
 * 08:30 - 14:59 = 8
 * 15:00 - 15:29 = 13
 * 15:30 - 16:59 = 18
 * 17:00 - 17:59 = 13
 * 18:00 - 18:29 = 8
 * 18:30 - 23:59 = 0
 */
object FeePriceRange {
    /**
     * 00:00 - 05:59
     * 18:30 - 23:59
     */
    val free = listOf(
        TimeSpan(
            start = TimeOfDay(0, 0),
            end = TimeOfDay(5, 59),
        ),
        TimeSpan(
            start = TimeOfDay(18, 30),
            end = TimeOfDay(23, 59),
        ),
    )

    /**
     * 06:00 - 06:29
     * 08:30 - 14:59
     * 18:00 - 18:29
     */
    val low = listOf(
        TimeSpan(
            start = TimeOfDay(6, 0),
            end = TimeOfDay(6, 29),
        ),
        TimeSpan(
            start = TimeOfDay(8, 30),
            end = TimeOfDay(14, 59)
        ),
        TimeSpan(
            start = TimeOfDay(18, 0),
            end = TimeOfDay(18, 29)
        ),
    )

    /**
     * 06:30 - 06:59
     * 08:00 - 08:29
     * 15:00 - 15:29
     * 17:00 - 17:59
     */
    val medium = listOf(
        TimeSpan(
            start = TimeOfDay(6, 30),
            end = TimeOfDay(6, 59),
        ),
        TimeSpan(
            start = TimeOfDay(8, 0),
            end = TimeOfDay(8, 29),
        ),
        TimeSpan(
            start = TimeOfDay(15, 0),
            end = TimeOfDay(15, 29),
        ),
        TimeSpan(
            start = TimeOfDay(17, 0),
            end = TimeOfDay(17, 59),
        ),
    )

    /**
     * 07:00 - 07:59
     * 15:30 - 16:59
     */
    val high = listOf(
        TimeSpan(
            start = TimeOfDay(7, 0),
            end = TimeOfDay(7, 59),
        ),
        TimeSpan(
            start = TimeOfDay(15, 30),
            end = TimeOfDay(16, 59),
        ),
    )
}
