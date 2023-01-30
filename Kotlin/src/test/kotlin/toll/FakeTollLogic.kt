package toll

import vehicle.Vehicle
import java.util.*

class FakeTollLogic(
    val feeFreeDays: List<Date> = emptyList(),
    val returnGroupByHour: List<List<Date>> = emptyList(),
    val returnIsInTimeSpan: Boolean = false
): TollLogic {
    override fun isTollFree(vehicle: Vehicle): Boolean {
        return vehicle.isTaxExempt
    }

    override fun isTollFreeDay(date: Date): Boolean {
        return feeFreeDays.contains(date)
    }

    override fun isWeekend(date: Date): Boolean {
        return feeFreeDays.contains(date)
    }

    override fun groupByHour(list: List<Date>): List<List<Date>> {
        return returnGroupByHour
    }

    override fun isInTimeSpan(date: Date, start: Date, end: Date): Boolean {
        return returnIsInTimeSpan
    }
}