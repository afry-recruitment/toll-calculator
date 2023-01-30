package time

import java.util.*

data class TimeOfDay(
    val hour: Int,
    val minute: Int
) {

    private fun getTotalMinutes(): Int = hour.times(60).plus(minute)

    /**
     * Check if value is between two times.
     */
    private fun between(start: TimeOfDay, end: TimeOfDay): Boolean {
        return start.getTotalMinutes() <= this.getTotalMinutes()
                && this.getTotalMinutes() <= end.getTotalMinutes()
    }

    fun inRange(ranges: List<TimeSpan>): Boolean = ranges.any { span ->
        this@TimeOfDay.between(span.start, span.end)
    }

    companion object {
        fun fromDate(
            date: Date,
            calendar: Calendar = GregorianCalendar.getInstance()
        ): TimeOfDay {
            calendar.time = date
            return TimeOfDay(
                hour = calendar.get(Calendar.HOUR_OF_DAY),
                minute = calendar.get(Calendar.MINUTE),
            )
        }
    }
}
