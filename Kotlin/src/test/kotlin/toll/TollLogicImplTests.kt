package toll

import org.junit.Test
import java.util.*

class TollLogicImplTests {

    private val logic = TollLogicImpl()

    @Test
    fun givenSingleTime_whenSortingByGroup_thenReturnSingleValue() {
        val cal = GregorianCalendar.getInstance()
        val expected = listOf(cal.time)
        val result = logic.groupByHour(expected)
        assert(result.flatten() == expected)
    }

    @Test
    fun givenMultipleTimesInOneHour_whenSortingByGroup_thenExpectGrouping() {
        val cal = GregorianCalendar.getInstance()
        val input = mutableListOf(
            cal.setHourAndMinute(1, 0).time,
            cal.setHourAndMinute(1, 1).time,
            cal.setHourAndMinute(1, 2).time,
        )
        val result = logic.groupByHour(input)
        assert(result.size == 1)
        assert(result.first() == input)
    }

    @Test
    fun givenMultipleDates_whenSortingByGroup_thenExpectMultipleGroups() {
        val cal = GregorianCalendar.getInstance()
        val input = mutableListOf(
            cal.setHourAndMinute(1, 1).time,
            cal.setHourAndMinute(2, 2).time,
            cal.setHourAndMinute(3, 3).time,
        )
        val result = logic.groupByHour(input)
        assert(result.size == 3)
        assert(result.flatten() == input)
    }

    @Test
    fun givenSpreadDates_whenGroupingItems_thenExpectMultipleGroups(){
        val cal = GregorianCalendar.getInstance()
        val input = mutableListOf(
            cal.setHourAndMinute(1, 0).time, // 1
            cal.setHourAndMinute(1, 1).time, // 1
            cal.setHourAndMinute(2, 1).time, // 2
            cal.setHourAndMinute(2, 2).time, // 2
            cal.setHourAndMinute(2, 3).time, // 2
            cal.setHourAndMinute(4, 3).time, // 3
        )
        val result = logic.groupByHour(input)
        assert(result.size == 3)
        assert(result[0] == listOf(input[0], input[1]))
        assert(result[1] == listOf(input[2], input[3], input[4]))
        assert(result[2] == listOf(input[5]))
    }

    private fun Calendar.setHourAndMinute(hour: Int, minute: Int): Calendar {
        set(Calendar.HOUR_OF_DAY, hour)
        set(Calendar.MINUTE, minute)
        return this
    }
}