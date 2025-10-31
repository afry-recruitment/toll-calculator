package com.assignment.toll_calculator.helpers;

import com.assignment.toll_calculator.utils.DateUtils;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

import static com.assignment.toll_calculator.utils.ConstantUtils.DATE_TIME_PATTERN;

/**
 * This class contains all supporting functions for the test cases.
 */
public class TestUtils {

    /**
     * Compose date based on parameters
     * @param month - the month
     * @param day - the day in month
     * @param hour - the hour of day
     * @param minute - the minute
     * @return - the composed date
     */
    public static Date composeDate(int month, int day, int hour, int minute) {
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.set(2013, month, day, hour, minute);
        return calendar.getTime();
    }

    /**
     * Compose date based on parameters
     * @param month - the month
     * @param day - the day in month
     * @return - the composed date
     */
    public static Date composeDate(int month, int day) {
        return composeDate(month, day, 9, 0);
    }

    /**
     * Compose date based on parameters
     * @param day - the day in year
     * @param hour - the hour of day
     * @param minute - the minute
     * @return - the composed date
     */
    public static Date composeDate(String day, int hour, int minute) {
        Date date = DateUtils.convert(day);

        Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);

        calendar.set(Calendar.HOUR_OF_DAY, hour);
        calendar.set(Calendar.MINUTE, minute);
        calendar.set(Calendar.SECOND, 0);

        return calendar.getTime();
    }

    /**
     * Format date based on a pattern
     * @param date - the date to format
     * @return - the formatted date
     */
    public static String format(Date date) {
        return new SimpleDateFormat(DATE_TIME_PATTERN).format(date);
    }

}
