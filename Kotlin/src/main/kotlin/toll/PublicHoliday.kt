package toll


data class MonthFeeFreeDays(
    val month: Month,
    val freeDaysInMonth: Set<Int> = emptySet()
)

enum class Month {
    January,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December,
}

fun createMonthFreeCalendar(
    january: Set<Int> = emptySet(),
    february: Set<Int> = emptySet(),
    march: Set<Int> = emptySet(),
    april: Set<Int> = emptySet(),
    may: Set<Int> = emptySet(),
    june: Set<Int> = emptySet(),
    july: Set<Int> = emptySet(),
    august: Set<Int> = emptySet(),
    september: Set<Int> = emptySet(),
    october: Set<Int> = emptySet(),
    november: Set<Int> = emptySet(),
    december: Set<Int> = emptySet(),
): List<MonthFeeFreeDays> = listOf(
    MonthFeeFreeDays(month = Month.January, january),
    MonthFeeFreeDays(month = Month.February, february),
    MonthFeeFreeDays(month = Month.March, march),
    MonthFeeFreeDays(month = Month.April, april),
    MonthFeeFreeDays(month = Month.May, may),
    MonthFeeFreeDays(month = Month.June, june),
    MonthFeeFreeDays(month = Month.July, july),
    MonthFeeFreeDays(month = Month.August, august),
    MonthFeeFreeDays(month = Month.September, september),
    MonthFeeFreeDays(month = Month.October, october),
    MonthFeeFreeDays(month = Month.November, november),
    MonthFeeFreeDays(month = Month.December, december),
)
