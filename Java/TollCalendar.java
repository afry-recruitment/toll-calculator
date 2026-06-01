import java.time.Month;
import java.time.MonthDay;
import java.util.HashSet;
import java.util.Set;

public class TollCalendar {
    private final Set<MonthDay> dateSet;
    private static TollCalendar instance;

    private TollCalendar() {
        dateSet = new HashSet<>();
        dateSet.add(MonthDay.of(Month.JANUARY, 1));
        dateSet.add(MonthDay.of(Month.MARCH, 28));
        dateSet.add(MonthDay.of(Month.MARCH, 29));
        dateSet.add(MonthDay.of(Month.APRIL, 1));
        dateSet.add(MonthDay.of(Month.APRIL, 30));
        dateSet.add(MonthDay.of(Month.MAY, 1));
        dateSet.add(MonthDay.of(Month.MAY, 8));
        dateSet.add(MonthDay.of(Month.MAY, 9));
        dateSet.add(MonthDay.of(Month.JUNE, 5));
        dateSet.add(MonthDay.of(Month.JUNE, 6));
        dateSet.add(MonthDay.of(Month.JUNE, 21));
        for (int d = 1; d <= Month.JULY.length(false); d++) {
            dateSet.add(MonthDay.of(Month.JULY, d));
        }
        dateSet.add(MonthDay.of(Month.NOVEMBER, 1));
        dateSet.add(MonthDay.of(Month.DECEMBER, 24));
        dateSet.add(MonthDay.of(Month.DECEMBER, 25));
        dateSet.add(MonthDay.of(Month.DECEMBER, 26));
        dateSet.add(MonthDay.of(Month.DECEMBER, 31));
    }

    public static TollCalendar getInstance() {
        if (instance == null) {
            instance = new TollCalendar();
        }
        return instance;
    }

    public boolean isTollFree(MonthDay monthDay) {
        return dateSet.contains(monthDay);
    }

    public boolean isTollFree(int month, int day){
        return isTollFree(MonthDay.of(month, day));
    }

    public Set<MonthDay> getDateSet(){
        return new HashSet<>(dateSet);
    }
}
