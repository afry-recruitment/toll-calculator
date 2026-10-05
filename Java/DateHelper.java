import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.Month;
import java.time.MonthDay;
import java.time.temporal.TemporalAdjusters;
import java.util.Set;

public class DateHelper {

    public static final Set<MonthDay> FIXED_HOLIDAYS = Set.of(
    MonthDay.of(Month.JANUARY, 1),    // New Year's Day
    MonthDay.of(Month.JANUARY, 6),    // Epiphany
    MonthDay.of(Month.MAY, 1),        // May Day
    MonthDay.of(Month.JUNE, 6),       // National Day
    MonthDay.of(Month.DECEMBER, 25),  // Christmas Day
    MonthDay.of(Month.DECEMBER, 26)   // Boxing Day
    );

    public static boolean isTollFreeDate(LocalDateTime localDateTime, boolean useGeneralSummerHoliday) {
        return isWeekend(localDateTime) || isFixedHoliday(localDateTime) || isDynamicHoliday(localDateTime)
                || (useGeneralSummerHoliday && isSummerHoliday(localDateTime));
    }

    private static boolean isWeekend(LocalDateTime localDateTime) {
        return localDateTime.getDayOfWeek() == java.time.DayOfWeek.SATURDAY || localDateTime.getDayOfWeek() == java.time.DayOfWeek.SUNDAY;
    }

    private static boolean isFixedHoliday(LocalDateTime localDateTime) {
        return FIXED_HOLIDAYS.contains(MonthDay.from(localDateTime)) || FIXED_HOLIDAYS.contains(MonthDay.from(localDateTime.plusDays(1)));
    }

    private static boolean isSummerHoliday(LocalDateTime localDateTime) {
        return localDateTime.getMonth() == Month.JULY;
    }

    private static boolean isDynamicHoliday(LocalDateTime localDateTime) {
        LocalDate date = localDateTime.toLocalDate();
        return isDynamicHolidayDate(date) || isDynamicHolidayDate(date.plusDays(1));
    }

    private static boolean isDynamicHolidayDate(LocalDate date) {
        int year = date.getYear();
        LocalDateTime easterSunday = calculateEasterSunday(year);
        LocalDateTime goodFriday = easterSunday.minusDays(2);
        LocalDateTime easterMonday = easterSunday.plusDays(1);
        LocalDateTime ascensionDay = easterSunday.plusDays(39);
        LocalDateTime whitSunday = easterSunday.plusDays(49);
        LocalDate midsummerDay = calculateMidsummerDay(year);
        LocalDate allSaintsDay = calculateAllSaintsDay(year);

        return date.equals(goodFriday.toLocalDate()) ||
               date.equals(easterMonday.toLocalDate()) ||
               date.equals(ascensionDay.toLocalDate()) ||
               date.equals(whitSunday.toLocalDate()) ||
               date.equals(midsummerDay) ||
               date.equals(allSaintsDay);
    }

    /*
     * Midsummer Day is the Saturday between June 20 and June 26.
     */
    private static LocalDate calculateMidsummerDay(int year) {
        return LocalDate.of(year, Month.JUNE, 20).with(TemporalAdjusters.nextOrSame(DayOfWeek.SATURDAY));
    }

    /*
     * All Saints' Day is the Saturday between October 31 and November 6.
     */
    private static LocalDate calculateAllSaintsDay(int year) {
        return LocalDate.of(year, Month.OCTOBER, 31).with(TemporalAdjusters.nextOrSame(DayOfWeek.SATURDAY));
    }

    /* 
     * Calculate the date of Easter Sunday.
     * Maybe more easy to use a library for this or some config file with the holidays for each year, but this is a simple implementation.   
     */
    private static LocalDateTime calculateEasterSunday(int year) {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;

        int month = (h + l - 7 * m + 114) / 31; // March=3, April=4
        int day = ((h + l - 7 * m + 114) % 31) + 1;

        return LocalDateTime.of(year, month, day, 0, 0);
    }
}
