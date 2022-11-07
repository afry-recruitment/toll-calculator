import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.Month;
import java.util.List;

public class TollFreeDateCheck {

    public boolean isTollFreeDate(LocalDate date) {
        if (isWeekend(date) || isJuly(date) || isHoliday(date)) return true;
        return false;
    }

    private boolean isWeekend(LocalDate date) {
        if (date.getDayOfWeek() == DayOfWeek.SATURDAY || date.getDayOfWeek() == DayOfWeek.SUNDAY) return true;
        return false;
    }

    private boolean isJuly(LocalDate date) {
        if (date.getMonth() == Month.JULY) return true;
        return false;
    }

    private boolean isHoliday(LocalDate date) {
        List<LocalDate> holidays = new Holidays().getHolidays(date.getYear());

        for (LocalDate holiday : holidays) {
            if (date == holiday) return true;
        }
        return false;
    }
}
