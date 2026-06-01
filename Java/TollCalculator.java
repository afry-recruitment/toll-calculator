
import java.time.DayOfWeek;
import java.time.Duration;
import java.time.LocalDateTime;
import java.time.MonthDay;
import java.time.ZoneId;
import java.util.*;

public class TollCalculator {

    private final TollingStrategy strategy;

    public TollCalculator(){
        strategy = new FirstPassStrategy();
    }

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public int getTollFee(Vehicle vehicle, Date... dates) {
        if (dates.length == 0) {
            return 0;
        }
        if (vehicle.isTollFree()) {
            return 0;
        }
        List<LocalDateTime> dateTimes = new ArrayList<>(dates.length);
        for (Date date : dates) {
            dateTimes.add(date.toInstant().atZone(ZoneId.systemDefault()).toLocalDateTime());
        }
        if (isTollFreeDate(dateTimes.get(0))) {
            return 0;
        }
        dateTimes.sort(Comparator.naturalOrder());
        return Math.min(60, strategy.getTollFee(vehicle, dateTimes));
    }

    private Boolean isTollFreeDate(LocalDateTime ldt) {
        DayOfWeek dayOfWeek = ldt.getDayOfWeek();
        if (dayOfWeek == DayOfWeek.SATURDAY || dayOfWeek == DayOfWeek.SUNDAY) {
            return true;
        }

        MonthDay monthDay = MonthDay.from(ldt);
        return TollCalendar.getInstance().isTollFree(monthDay);
    }
}
