import toll.TollCalculator
import vehicle.*
import java.util.*

fun main(args: Array<String>) {
    val vehicles = listOf(
        Car(),
        Diplomat(),
        Emergency(),
        Foreign(),
        Military(),
        Motorbike(),
        Tractor(),
    )
    val calendar = GregorianCalendar.getInstance()
    val calculator = TollCalculator()
    vehicles.forEach { vehicle ->
        calculator.getTollFee(
            vehicle = vehicle,
            date = calendar.time
        )
    }
}