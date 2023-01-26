import java.util.*
import java.util.concurrent.TimeUnit

class TollCalculator {
    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    fun getTollFee(vehicle: Vehicle, vararg dates: Date): Int {
        val intervalStart = dates[0]
        var totalFee = 0
        for (date in dates) {
            val nextFee = getTollFee(date, vehicle)
            var tempFee = getTollFee(intervalStart, vehicle)
            val timeUnit = TimeUnit.MINUTES
            val diffInMillies = date.time - intervalStart.time
            val minutes = timeUnit.convert(diffInMillies, TimeUnit.MILLISECONDS)
            if (minutes <= 60) {
                if (totalFee > 0) totalFee -= tempFee
                if (nextFee >= tempFee) tempFee = nextFee
                totalFee += tempFee
            } else {
                totalFee += nextFee
            }
        }
        if (totalFee > 60) totalFee = 60
        return totalFee
    }

    private fun isTollFreeVehicle(vehicle: Vehicle): Boolean {
        if (vehicle == null) return false
        val vehicleType = vehicle.getType()
        return vehicleType == TollFreeVehicles.MOTORBIKE.type
                || vehicleType == TollFreeVehicles.TRACTOR.type
                || vehicleType == TollFreeVehicles.EMERGENCY.type
                || vehicleType == TollFreeVehicles.DIPLOMAT.type
                || vehicleType == TollFreeVehicles.FOREIGN.type
                || vehicleType == TollFreeVehicles.MILITARY.type
    }

    fun getTollFee(date: Date, vehicle: Vehicle): Int {
        if (isTollFreeDate(date) || isTollFreeVehicle(vehicle)) return 0
        val calendar = GregorianCalendar.getInstance()
        calendar.time = date
        val hour = calendar[Calendar.HOUR_OF_DAY]
        val minute = calendar[Calendar.MINUTE]
        return if (hour == 6 && minute >= 0&& minute <= 29) 8
        else if (hour == 6 && minute >= 30 && minute <= 59) 13
        else if (hour == 7 && minute >= 0 && minute <= 59) 18
        else if (hour == 8 && minute >= 0 && minute <= 29) 13
        else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) 8
        else if (hour == 15 && minute >= 0 && minute <= 29) 13
        else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) 18
        else if (hour == 17 && minute >= 0 && minute <= 59) 13
        else if (hour == 18 && minute >= 0 && minute <= 29) 8
        else 0
    }

    private fun isTollFreeDate(date: Date): Boolean {
        val calendar = GregorianCalendar.getInstance()
        calendar.time = date
        val year = calendar[Calendar.YEAR]
        val month = calendar[Calendar.MONTH]
        val day = calendar[Calendar.DAY_OF_MONTH]
        val dayOfWeek = calendar[Calendar.DAY_OF_WEEK]
        if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) return true
        if (year == 2013) {
            if (month == Calendar.JANUARY && day == 1
                || month == Calendar.MARCH && (day == 28 || day == 29)
                || month == Calendar.APRIL && (day == 1 || day == 30)
                || month == Calendar.MAY && (day == 1 || day == 8 || day == 9)
                || month == Calendar.JUNE && (day == 5 || day == 6 || day == 21)
                || month == Calendar.JULY
                || month == Calendar.NOVEMBER && day == 1
                || month == Calendar.DECEMBER && (day == 24 || day == 25 || day == 26 || day == 31)
            ) {
                return true
            }
        }
        return false
    }

    private enum class TollFreeVehicles(val type: String) {
        MOTORBIKE("Motorbike"),
        TRACTOR("Tractor"),
        EMERGENCY("Emergency"),
        DIPLOMAT("Diplomat"),
        FOREIGN("Foreign"),
        MILITARY("Military");
    }
}