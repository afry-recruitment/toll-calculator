package com.afry.tollcalculator;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.Month;
import java.time.ZoneId;
import java.util.Date;

public class HolidayUtils {

    public static boolean isWeekend(Date date) {
        LocalDate localDate = LocalDate.ofInstant(date.toInstant(), ZoneId.systemDefault());
        DayOfWeek dayOfWeek = localDate.getDayOfWeek();

        return (DayOfWeek.SATURDAY.equals(dayOfWeek)
                || DayOfWeek.SUNDAY.equals(dayOfWeek));
    }

    public static boolean isInJuly(Date date) {
        LocalDate localDate = LocalDate.ofInstant(date.toInstant(), ZoneId.systemDefault());
        Month month = localDate.getMonth();

        return (Month.JULY.equals(month));
    }

    public static boolean isSwedishFixedHoliday(Date date) {
        LocalDate localDate = LocalDate.ofInstant(date.toInstant(), ZoneId.systemDefault());
        Month month = localDate.getMonth();
        int dayOfMonth = localDate.getDayOfMonth();

        return ((Month.JANUARY.equals(month) && dayOfMonth == 1) // New Year
                || (Month.JANUARY.equals(month) && dayOfMonth == 6) // Epiphany
                || (Month.MAY.equals(month) && dayOfMonth == 1) // Labor Day
                || (Month.JUNE.equals(month) && dayOfMonth == 6) // National Day
                || (Month.DECEMBER.equals(month) && dayOfMonth == 25) // Christmas Day
                || (Month.DECEMBER.equals(month) && dayOfMonth == 26)); // Boxing Day
    }

    public static boolean isDayBeforeHoliday(Date date) {
        LocalDate localDate = LocalDate.ofInstant(date.toInstant(), ZoneId.systemDefault());
        LocalDate dayAfter = localDate.plusDays(1);
        Date dateDayAfter = Date.from(dayAfter.atStartOfDay(ZoneId.systemDefault()).toInstant());

        return isSwedishFixedHoliday(dateDayAfter) || isEastern(dateDayAfter)
                || isAllSaintsDay(dateDayAfter) || isAscensionDay(dateDayAfter);
    }

    public static boolean isAllSaintsDay(Date date) {
        LocalDate localDate = LocalDate.ofInstant(date.toInstant(), ZoneId.systemDefault());
        Month month = localDate.getMonth();
        DayOfWeek dayOfWeek = localDate.getDayOfWeek();
        int day = localDate.getDayOfMonth();

        // All Saints Day (Saturday between(including) Oct 31 and Nov 6)
        boolean november1To6 = (Month.NOVEMBER.equals(month) && (day >= 1 && day <= 6));
        boolean october31 = (Month.OCTOBER.equals(month) && day == 31);

        return DayOfWeek.SATURDAY.equals(dayOfWeek) && (october31 || november1To6);
    }

    public static boolean isMidsummerEve(Date date) {
        LocalDate localDate = LocalDate.ofInstant(date.toInstant(), ZoneId.systemDefault());
        Month month = localDate.getMonth();
        DayOfWeek dayOfWeek = localDate.getDayOfWeek();
        int day = localDate.getDayOfMonth();

        // Midsummer's Eve (Friday between June 19 and June 25)
        return Month.JUNE.equals(month)
                && DayOfWeek.FRIDAY.equals(dayOfWeek)
                && (day >= 19 && day <= 25);
    }

    public static boolean isAscensionDay(Date date) {
        LocalDate localDate = LocalDate.ofInstant(date.toInstant(), ZoneId.systemDefault());
        int year = localDate.getYear();

        // Calculate Easter Sunday (for Easter Monday and Good Friday)
        LocalDate easterSunday = calculateEasterSunday(year);

        return localDate.equals(easterSunday.plusDays(39));
    }

    public static boolean isEastern(Date date) {

        LocalDate localDate = LocalDate.ofInstant(date.toInstant(), ZoneId.systemDefault());
        int year = localDate.getYear();

        // Calculate Easter Sunday (for Easter Monday and Good Friday)
        LocalDate easterSunday = calculateEasterSunday(year);

        return (localDate.equals(easterSunday.plusDays(1))
                || localDate.equals(easterSunday)
                || localDate.equals(easterSunday.minusDays(1))
                || localDate.equals(easterSunday.minusDays(2)));

    }

    // Baeldung algoritm
    private static LocalDate calculateEasterSunday(int year) {
        int a = year % 19;
        int b = year % 4;
        int c = year % 7;
        int k = year / 100;
        int p = (13 + 8 * k) / 25;
        int q = k / 4;
        int M = (15 - p + k - q) % 30;
        int N = (4 + k - q) % 7;
        int d = (19 * a + M) % 30;
        int e = (2 * b + 4 * c + 6 * d + N) % 7;

        if (d == 29 && e == 6) {
            return LocalDate.of(year, 4, 19);
        } else if (d == 28 && e == 6 && ((11 * M + 11) % 30 < 10)) {
            return LocalDate.of(year, 4, 18);
        }

        int H = 22 + d + e;
        if (H <= 31) {
            return LocalDate.of(year, 3, H);
        }
        return LocalDate.of(year, 4, H - 31);
    }

}

