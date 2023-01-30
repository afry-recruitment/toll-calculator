package toll

import time.FreeTollDays
import vehicle.Vehicle
import java.util.*

class TollLogicImpl : TollLogic {
    override fun isTollFree(vehicle: Vehicle): Boolean = vehicle.isTaxExempt

    override fun isTollFreeDay(date: Date): Boolean {
        val calendar = GregorianCalendar.getInstance().apply { time = date }
        val holidays = FreeTollDays.forYear(calendar.get(Calendar.YEAR))
        val day = calendar.get(Calendar.DAY_OF_MONTH)
        val month = calendar.get(Calendar.MONTH)
        return holidays[month].freeDaysInMonth.contains(day)
    }

    override fun isWeekend(date: Date): Boolean {
        val dayOfWeek = GregorianCalendar
            .getInstance()
            .apply { time = date }
            .get(Calendar.DAY_OF_WEEK)
        return dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY
    }

    /**
     * Groups all the dates with the first date as a starting point, i.e. does not reset on the hour
     * every hour but rather one hour after the initial date
     *
     * @param dates a sorted list of dates to be segmented into groups.
     */
    override fun groupByHour(dates: List<Date>): List<List<Date>> {
        if (dates.size < 2) {
            return listOf(dates)
        }

        val initial = dates.first()
        val current = GregorianCalendar.getInstance().apply { time = initial }
        current.add(Calendar.HOUR, 1)

        val groups = mutableListOf(mutableListOf(initial))

        for(time in dates.drop(1)){

            if(current.time.after(time)){
                groups.last().add(time)
            } else {
                current.apply {
                    this.time = time
                    add(Calendar.HOUR, 1)
                }
                groups.add(mutableListOf(time))
            }
        }

        return groups
    }

    override fun isInTimeSpan(date: Date, start: Date, end: Date): Boolean {
        return date == start ||
                date == end ||
                date.after(start) && date.before(end)
    }
}
