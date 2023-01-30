package toll

import vehicle.Vehicle
import java.util.*

interface TollLogic {
    fun isTollFree(vehicle: Vehicle): Boolean
    fun isTollFreeDay(date: Date): Boolean
    fun isWeekend(date: Date): Boolean
    fun groupByHour(dates: List<Date>): List<List<Date>>
    fun isInTimeSpan(date: Date, start: Date, end: Date): Boolean
}
