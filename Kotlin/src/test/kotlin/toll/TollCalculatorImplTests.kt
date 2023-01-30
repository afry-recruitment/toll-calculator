package toll

import org.junit.Assert
import org.junit.Before
import org.junit.Test
import vehicle.Diplomat
import vehicle.Vehicle
import java.util.*

class TollCalculatorImplTests {

    private lateinit var calculator: TollCalculatorImpl

    @Before
    fun setup() {
        calculator = TollCalculatorImpl()
    }

    @Test
    fun givenTaxFreeVehicle_whenCalculatingTollFee_thenExpectFree() {
        val taxExemptVehicle = Diplomat()
        val logic = FakeTollLogic()
        assert(logic.isTollFree(vehicle = taxExemptVehicle))
    }

    /**
     * Test boundaries for free toll zones
     */
    @Test
    fun givenTaxedVehicleDuringFreeHours_whenCalculatingTollFee_thenExpectFreeToll() {
        val fakeTaxedCar = FakeVehicles.taxedCar

        // 00:00 - 05:59
        fakeTaxedCar.getTollPrice(hour = 0, minute = 0, expectedPrice = TollPrice.Free)
        fakeTaxedCar.getTollPrice(hour = 5, minute = 59, expectedPrice = TollPrice.Free)

        // 18:30 - 23:59
        fakeTaxedCar.getTollPrice(hour = 18, minute = 30, expectedPrice = TollPrice.Free)
        fakeTaxedCar.getTollPrice(hour = 23, minute = 59, expectedPrice = TollPrice.Free)
    }

    /**
     * Test boundaries for low toll zones
     */
    @Test
    fun givenTaxedVehicleDuringLowPriceHours_whenCalculatingFee_thenExpectLowPrice() {
        val taxedCar = FakeVehicles.taxedCar

        // 06:00 - 06:29
        taxedCar.getTollPrice(hour = 6, minute = 0, expectedPrice = TollPrice.Low)
        taxedCar.getTollPrice(hour = 6, minute = 29, expectedPrice = TollPrice.Low)

        // 08:30 - 14:59
        taxedCar.getTollPrice(hour = 8, minute = 30, expectedPrice = TollPrice.Low)
        taxedCar.getTollPrice(hour = 14, minute = 59, expectedPrice = TollPrice.Low)

        // 18:00 - 18:29
        taxedCar.getTollPrice(hour = 18, minute = 0, expectedPrice = TollPrice.Low)
        taxedCar.getTollPrice(hour = 18, minute = 29, expectedPrice = TollPrice.Low)
    }

    /**
     * Test boundaries for free medium zones
     */
    @Test
    fun givenTaxedVehicleDuringMediumHours_whenCalculatingFee_thenExpectMediumPrice() {
        val taxedCar = FakeVehicles.taxedCar

        // 06:30 - 06:59
        taxedCar.getTollPrice(hour = 6, minute = 30, expectedPrice = TollPrice.Medium)
        taxedCar.getTollPrice(hour = 6, minute = 59, expectedPrice = TollPrice.Medium)

        // 08:00 - 08:29
        taxedCar.getTollPrice(hour = 8, minute = 0, expectedPrice = TollPrice.Medium)
        taxedCar.getTollPrice(hour = 8, minute = 29, expectedPrice = TollPrice.Medium)

        // 15:00 - 15:29
        taxedCar.getTollPrice(hour = 15, minute = 0, expectedPrice = TollPrice.Medium)
        taxedCar.getTollPrice(hour = 15, minute = 29, expectedPrice = TollPrice.Medium)

        // 17:00 - 17:59
        taxedCar.getTollPrice(hour = 17, minute = 0, expectedPrice = TollPrice.Medium)
        taxedCar.getTollPrice(hour = 17, minute = 59, expectedPrice = TollPrice.Medium)
    }

    /**
     * Test boundaries for free high zones
     */
    @Test
    fun givenTaxedVehicleDuringRushHours_whenCalculatingFee_thenExpectHighPrice() {
        val taxedCar = FakeVehicles.taxedCar

        // 07:00 - 07:59
        taxedCar.getTollPrice(hour = 7, minute = 0, expectedPrice = TollPrice.High)
        taxedCar.getTollPrice(hour = 7, minute = 59, expectedPrice = TollPrice.High)

        // 15:30 - 16:59
        taxedCar.getTollPrice(hour = 15, minute = 30, expectedPrice = TollPrice.High)
        taxedCar.getTollPrice(hour = 16, minute = 59, expectedPrice = TollPrice.High)
    }

    private fun Vehicle.getTollPrice(hour: Int, minute: Int, expectedPrice: TollPrice) {
        val calendar = createCalendarWithTimeAndDate()
        calculator.calculateTollFee(
            vehicle = this,
            date = calendar.apply {
                set(Calendar.HOUR_OF_DAY, hour)
                set(Calendar.MINUTE, minute)
            }.time
        ).takeIf { price -> price == expectedPrice.price }
            ?: error("Expected price to be $expectedPrice")
    }

    /**
     * Test all times of the day, ensure that they all are represented by a correlated price.
     */
    @Test
    fun givenAllMinutesOfTheDay_whenCalculatingPrice_thenExpectEveryMinuteToHaveAPrice() {
        val vehicle = FakeVehicles.taxedCar
        val calendar = GregorianCalendar.getInstance()
        for (hour in 0..23) {
            for (minute in 0..59) {
                calculator.calculateTollFee(
                    vehicle = vehicle,
                    date = calendar.apply {
                        set(Calendar.HOUR_OF_DAY, hour)
                        set(Calendar.MINUTE, minute)
                    }.time
                )
            }
        }
    }

    @Test
    fun givenMultipleFeesAnHour_whenCalculatingPrice_thenExcludeLesserFees() {
        val vehicle = FakeVehicles.taxedCar
        val calendar = createCalendarWithTimeAndDate(
            hour = 6,
            minute = 55,
            month = Calendar.FEBRUARY,
            year = 2013
        )

        val dates = mutableListOf(calendar.time)
        // get one fee every minute for 10 minutes, price should increase when rush time occurs.
        for (i in 0..10) {
            calendar.add(Calendar.MINUTE, 1)
            dates.add(calendar.time)
        }


        val result = calculator.calculateTollFee(
            vehicle = vehicle,
            dates = dates
        )

        assert(result == TollPrice.High.price)
    }

    @Test
    fun givenMultipleFeedOverSeveralHours_whenCalculatingPrice_thenAddAllTogether() {
        val vehicle = FakeVehicles.taxedCar
        val calendar = GregorianCalendar.getInstance().apply {
            set(Calendar.YEAR, 2013)
            set(Calendar.MONTH, Calendar.FEBRUARY)
            set(Calendar.HOUR_OF_DAY, 6)
            set(Calendar.MINUTE, 55)
        }

        val dates = mutableListOf(calendar.time)
        // get one fee every minute for 10 minutes, price should increase when rush time occurs.
        for (i in 0..3) {
            calendar.add(Calendar.HOUR_OF_DAY, 1)
            dates.add(calendar.time)
        }

        val result = calculator.calculateTollFee(
            vehicle = vehicle,
            dates = dates
        )

        // Medium + high + low + low + low
        Assert.assertEquals(55, result)
    }

    private fun createCalendarWithTimeAndDate(
        hour: Int = 0,
        minute: Int = 0,
        month: Int = Calendar.JANUARY,
        year: Int = 2023 // important to determine free days for any given year
    ): Calendar {
        return GregorianCalendar.getInstance().apply {
            set(Calendar.HOUR_OF_DAY, hour)
            set(Calendar.MINUTE, minute)
            set(Calendar.MONTH, month)
            set(Calendar.YEAR, year)
        }
    }

    @Test
    fun givenTollFreeDay_whenCalculatingTollFee_thenReturnFreePrice() {
        val calendar = createCalendarWithTimeAndDate(hour = 9)
        val fakeLogic = FakeTollLogic(feeFreeDays = listOf(calendar.time))
        val fakeCalculator = TollCalculatorImpl(tollLogic = fakeLogic)
        val vehicle = FakeVehicles.taxedCar
        val cost = fakeCalculator.calculateTollFee(vehicle = vehicle, date = calendar.time)
        Assert.assertEquals(TollPrice.Free.price, cost)
    }

    @Test
    fun givenTollFeesOverMaxAmount_whenCalculatingFees_thenReturnMaxAmount() {
        val calendar = createCalendarWithTimeAndDate()
        val vehicle = FakeVehicles.taxedCar
        val dates = mutableListOf<Date>()
        // get vehicle fees for every minute of every day, should only amount to 60
        for (hour in 0..23) {
            for (minute in 0..59) {
                calendar.apply {
                    set(Calendar.HOUR_OF_DAY, hour)
                    set(Calendar.MINUTE, minute)
                }
                dates.add(calendar.time)
            }
        }
        val result = calculator.calculateTollFee(vehicle, dates)
        Assert.assertEquals(TollCalculatorImpl.MAXIMUM_DAILY_FEE_PRICE, result)
    }

    @Test(expected = IllegalStateException::class)
    fun givenDatesStretchingOverSeveralDays_whenCalculatingFees_thenExpectError(){
        val calendar = createCalendarWithTimeAndDate()
        val vehicle = FakeVehicles.taxedCar
        val dates = mutableListOf<Date>()
        for(day in 0..3){
            calendar.set(Calendar.DAY_OF_YEAR, day)
            dates.add(calendar.time)
        }
        calculator.calculateTollFee(vehicle = vehicle, dates = dates)
    }
}
