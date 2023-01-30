package toll

import di.AppDependencyProvider
import time.TimeOfDay
import time.FeePriceRange
import vehicle.Vehicle
import java.util.*

interface TolLCalculator {
    fun calculateTollFee(vehicle: Vehicle, date: Date): Int
    fun calculateTollFee(vehicle: Vehicle, dates: List<Date>): Int
}

class TollCalculatorImpl(
    private val tollLogic: TollLogic = AppDependencyProvider.provideTollLogic()
) : TolLCalculator {

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    override fun calculateTollFee(
        vehicle: Vehicle,
        dates: List<Date>
    ): Int = dates
        .ensureSingleDay()                          // calculator is limited to only handle a single day
        .emptyIfVehicleTollFeeExempt(vehicle = vehicle)    // disregard operation if vehicle is exempt from fees
        .filter { date -> !tollLogic.isTollFreeDay(date) } // remove fee free days
        .groupedByHour()                            // group items
        .byHighestPricePerHour(vehicle = vehicle)   // find the max price for each hour.
        .sumOf { it.price }                         // summarize total price
        .coerceAtMost(MAXIMUM_DAILY_FEE_PRICE)      // limit to 60 per day

    /**
     * Helper function to translate single date object to list.
     */
    override fun calculateTollFee(vehicle: Vehicle, date: Date): Int = calculateTollFee(
        vehicle = vehicle,
        dates = listOf(date)
    )

    /**
     * The calculator is bound to
     */
    private fun List<Date>.ensureSingleDay(): List<Date> {
        val calendar = GregorianCalendar.getInstance()
        val groups = groupBy {
            calendar.time = it
            calendar.get(Calendar.DAY_OF_YEAR)
        }

        if(groups.size > 1) {
            error("Calculator can only handle a single day")
        }

        return this
    }

    /**
     * Helper extension removing all dates if vehicle is exempt from toll fees.
     */
    private fun List<Date>.emptyIfVehicleTollFeeExempt(vehicle: Vehicle): List<Date> =
        if (vehicle.isTaxExempt) emptyList() else this

    /**
     * Wrapped for readability.
     */
    private fun List<Date>.groupedByHour(): List<List<Date>> = tollLogic.groupByHour(dates = this)

    /**
     * Identify the time with the highest price for each hour.
     */
    private fun List<List<Date>>.byHighestPricePerHour(
        vehicle: Vehicle
    ): List<TollPrice> {
        return map { hourly ->
            hourly.map { date -> getTollFeeForDate(vehicle = vehicle, date = date) }
                .maxByOrNull { it.price }
                ?: TollPrice.Free
        }
    }

    /**
     * Determine the fee price for any given time of day.
     */
    private fun getTollFeeForDate(
        vehicle: Vehicle,
        date: Date,
    ): TollPrice {
        if (vehicle.isTaxExempt) {
            return TollPrice.Free
        }

        val timeOfDay = TimeOfDay.fromDate(
            date = date,
            calendar = GregorianCalendar.getInstance()
        )

        return when {
            timeOfDay.inRange(FeePriceRange.free) -> TollPrice.Free
            timeOfDay.inRange(FeePriceRange.low) -> TollPrice.Low
            timeOfDay.inRange(FeePriceRange.medium) -> TollPrice.Medium
            timeOfDay.inRange(FeePriceRange.high) -> TollPrice.High
            else -> error("time $timeOfDay is not covered by any fees")
        }
    }

    companion object {
        const val MAXIMUM_DAILY_FEE_PRICE: Int = 60
    }
}
