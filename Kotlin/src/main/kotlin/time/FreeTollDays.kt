package time

import toll.MonthFeeFreeDays
import toll.createMonthFreeCalendar

object FreeTollDays {

    /**
     * Defining fee free days should be provided by the backend and not managed by the static client
     * should they ever need to be updated.
     */
    fun forYear(year: Int): List<MonthFeeFreeDays> {
        return when (year) {
            // todo: Verify that days of 2023 are the same as 2013
            2023 -> createMonthFreeCalendar(
                january = setOf(1),
                march = setOf(28, 29),
                april = setOf(1, 30),
                may = setOf(1, 8, 9),
                june = setOf(5, 6, 21),
                july = (1..31).toSet(),
                november = setOf(1),
                december = setOf(24, 25, 26, 31)
            )

            2013 -> createMonthFreeCalendar(
                january = setOf(1),
                march = setOf(28, 29),
                april = setOf(1, 30),
                may = setOf(1, 8, 9),
                june = setOf(5, 6, 21),
                july = (1..31).toSet(),
                november = setOf(1),
                december = setOf(24, 25, 26, 31)
            )

            else -> error("Found no public holidays for $year")
        }
    }
}
