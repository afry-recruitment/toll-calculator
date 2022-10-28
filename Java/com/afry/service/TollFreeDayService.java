package com.afry.service;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class TollFreeDayService {

    private enum GenericHoliday {
        NEW_YEARS_DAY("Jan 1"),
        EPIPHANY("Jan 6"),
        LABOUR_DAY("May 1"),
        NATIONAL_DAY("Jun 6"),
        CHRISTMAS_EVE("Dec 24"),
        CHRISTMAS_DAY("Dec 25"),
        ST_STEPHENS_DAY("Dec 26"),
        NEW_YEARS_EVE("Dec 31");

        private final String date;

        GenericHoliday(String date) {
            this.date = date;
        }

        public String getDate() {
            return date;
        }
    }

    public boolean isTollFreeDay(LocalDateTime dateTime) {
        return isWeekend(dateTime) || isSwedenHoliday(dateTime);
    }

    public boolean isWeekend(LocalDateTime dateTime) {
        DayOfWeek dayOfWeek = dateTime.getDayOfWeek();
        return DayOfWeek.SATURDAY.equals(dayOfWeek) || DayOfWeek.SUNDAY.equals(dayOfWeek);
    }

    public boolean isSwedenHoliday(LocalDateTime dateTime) {
        try {
            if (isGenericHoliday(dateTime)) {
                return true;
            } else {

                String easterDay = calculateEasterDayForTheYear(dateTime.getYear());
                DateTimeFormatter formatter = DateTimeFormatter.ofPattern("yyyy-MM-dd");
                LocalDate easterDate = LocalDate.parse(easterDay, formatter);
                LocalDate date = dateTime.toLocalDate();
                LocalDate goodFriday = easterDate.minusDays(2);
                LocalDate easterMonday = easterDate.plusDays(1);
                LocalDate ascensionDay = easterDate.plusDays(39);

                //WhitSunday and EasterDay are not explicitly calculated, as it is a Sunday and already considered as tollFree day
                return goodFriday.isEqual(date) ||
                        easterMonday.isEqual(date) ||
                        ascensionDay.isEqual(date);
            }
        } catch (Exception E) {
            System.out.println("Exception caught during parsing of Easter Day" + E.getMessage());
            return false;
        }
    }

    public boolean isGenericHoliday(LocalDateTime dateTime) {
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern("MMM d");
        String date = dateTime.format(formatter);
        return GenericHoliday.NEW_YEARS_DAY.getDate().equals(date) ||
                GenericHoliday.EPIPHANY.getDate().equals(date) ||
                GenericHoliday.LABOUR_DAY.getDate().equals(date) ||
                GenericHoliday.NATIONAL_DAY.getDate().equals(date) ||
                GenericHoliday.CHRISTMAS_EVE.getDate().equals(date) ||
                GenericHoliday.CHRISTMAS_DAY.getDate().equals(date) ||
                GenericHoliday.ST_STEPHENS_DAY.getDate().equals(date) ||
                GenericHoliday.NEW_YEARS_EVE.getDate().equals(date);
    }

    /**
     * This method calculates the Easter sunday as per the Gauss Easter Algorithm.
     *
     * @param year - year in yyyy format
     * @return string values which represents the Easter sunday
     */
    public String calculateEasterDayForTheYear(int year) {
        float locationOfMetCycle, numberOfLeapDays, numberOfDaysPast52Weeks;

        //Variables to define the century value
        float centDepVal1, centDepVal2, centuryDependentVal;

        //Variable to find the difference between the number of leap days between the Julian and the Gregorian calendar
        float noOfLeapDays;

        float daysToPaschalFullMoon, daysToEaster;

        locationOfMetCycle = year % 19;
        numberOfLeapDays = year % 4;
        numberOfDaysPast52Weeks = year % 7;

        //For 19th century, centuryDependentVal = 23. For the 21st century, centuryDependentVal = 24 and so on.
        centDepVal1 = (float) Math.floor(year / 100);
        centDepVal2 = (float) Math.floor((13 + 8 * centDepVal1) / 25);
        centuryDependentVal = (int) (15 - centDepVal2 + centDepVal1 - Math.floor(centDepVal1 / 4)) % 30;

        noOfLeapDays = (int) (4 + centDepVal1 - Math.floor(centDepVal1 / 4)) % 7;
        daysToPaschalFullMoon = (19 * locationOfMetCycle + centuryDependentVal) % 30;
        daysToEaster = (2 * numberOfLeapDays + 4 * numberOfDaysPast52Weeks + 6 * daysToPaschalFullMoon + noOfLeapDays) % 7;
        int days = (int) (22 + daysToPaschalFullMoon + daysToEaster);

        // corner case, when D is 29
        if ((daysToPaschalFullMoon == 29) && (daysToEaster == 6)) {
            return year + "-04" + "-19";
        }
        // Another corner case,when D is 28
        else if ((daysToPaschalFullMoon == 28) && (daysToEaster == 6)) {
            return year + "-04" + "-18";
        } else {
            // If days > 31, then move to April = 4th Month
            if (days > 31) {
                return (days - 31 < 10) ? year + "-04-0" + (days - 31) : year + "-04-" + (days - 31);
            }
            // Otherwise, stay on March = 3rd Month
            else {
                return year + "-03-" + days;
            }
        }
    }
}


