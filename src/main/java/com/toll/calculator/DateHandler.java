package com.toll.calculator;

import de.jollyday.Holiday;
import de.jollyday.HolidayManager;
import de.jollyday.ManagerParameter;
import de.jollyday.ManagerParameters;

import java.time.*;
import java.util.*;

import static com.toll.calculator.Utils.*;

public class DateHandler {
    private final Set<LocalDate> holidayDateSet;
    private static final ZoneId zoneId = ZoneId.of(TIMEZONE);
    private final int year;

    // Manual holiday creation for year 2023
    public DateHandler() {
        year = 2023;
        Set<LocalDate> dateSet = new HashSet<>(Arrays.asList(holidayDatesForYear2023));
        holidayDateSet = Collections.unmodifiableSet(dateSet);
    }

    // Used for dynamically holiday fetching via API (NOTE: Java 8)
    public DateHandler(int year) {
        this.year = year;
        Locale sweden = new Locale(LOCALE_LANGUAGE, LOCALE_COUNTRY);
        ManagerParameter params = ManagerParameters.create(sweden);
        HolidayManager m = HolidayManager.getInstance(params);

        Set<Holiday> holidaySet = m.getHolidays(year);
        holidayDateSet = Utils.getHolidaysAsDates(holidaySet);
    }


    // https://vision.se/tidningenvision/arkiv/2022/nr6/sa-manga-roda-dagar-ar-det-2023/
    // Holiday dates for 2023 in Sweden
    public static LocalDate[] holidayDatesForYear2023 = {
            LocalDate.of(2023, 1,1),
            LocalDate.of(2023, 1,6),
            LocalDate.of(2023, 4,7),
            LocalDate.of(2023, 4,9),
            LocalDate.of(2023, 4,10),
            LocalDate.of(2023, 5,1),
            LocalDate.of(2023, 5,18),
            LocalDate.of(2023, 5,28),
            LocalDate.of(2023, 6,6),
            LocalDate.of(2023, 6,23),
            LocalDate.of(2023, 6,24),
            LocalDate.of(2023, 11,4),
            LocalDate.of(2023, 11,24),
            LocalDate.of(2023, 12,25),
            LocalDate.of(2023, 12,26),
            LocalDate.of(2023, 12,31),
    };

    public Set<LocalDate> getHolidayDateSet() {
        return holidayDateSet;
    }

    public static boolean isWeekendDay(int dayOfWeek) {
        return dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY;
    }

    public static LocalDate getLocalDateFromDate(Date date) {
        return date.toInstant().atZone(zoneId).toLocalDate();
    }

    public static LocalTime getLocalTimeFromDate(Date date) {
        return LocalDateTime.ofInstant(date.toInstant(), zoneId).toLocalTime();
    }

    public static boolean isLeapYear(int year) {
        return Year.isLeap(year);
    }

    // Gets randomized dates for the set year
    public Date[] getRandomizedDates(int nrDates) {
        Date[] dates = new Date[nrDates];
        int randomizeMonthNumber = Utils.randomizeInt(Month.DECEMBER.getValue());
        Month randomizedMonth = Month.values()[randomizeMonthNumber];
        boolean isLeapYear = isLeapYear(year);
        int randomizedMonthDayNumber = Utils.randomizeInt(randomizedMonth.length(isLeapYear)) + 1;

        LocalDate randomizedLocalDate = LocalDate.of(year, randomizedMonth, randomizedMonthDayNumber);
        int hoursOfDay = 24, minutesOfDay = 60;
        for (int i = 0; i < nrDates; i++) {
            int randomizedHour = Utils.randomizeInt(hoursOfDay);
            int randomizedMinutes = Utils.randomizeInt(minutesOfDay);
            LocalTime randomizedTime = LocalTime.of(randomizedHour, randomizedMinutes);

            Instant instant = randomizedTime.atDate(randomizedLocalDate).atZone(zoneId).toInstant();
            Date randomizedDate = Date.from(instant);
            dates[i] = randomizedDate;
        }
        return dates;
    }
}
