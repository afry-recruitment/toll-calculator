import di.AppDependencyProvider
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
    val calculator = AppDependencyProvider.provideTollCalculator()
    vehicles.forEach { vehicle ->
        calculator.calculateTollFee(
            vehicle = vehicle,
            date = calendar.time
        )
    }
}