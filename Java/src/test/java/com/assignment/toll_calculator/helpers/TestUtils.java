package com.assignment.toll_calculator.helpers;

import com.assignment.toll_calculator.utils.DateUtils;

import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

/**
 * This class contains all supporting functions for the test cases.
 */
public class TestUtils {

    public static Date composeDate(int month, int day, int hour, int minute) {
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.set(2013, month, day, hour, minute);
        return calendar.getTime();
    }

    public static Date composeDate(int month, int day) {
        return composeDate(month, day, 9, 0);
    }

    public static Date composeDate(String day, int hour, int minute) {
        Date date = DateUtils.convert(day);

        Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);

        calendar.set(Calendar.HOUR_OF_DAY, hour);
        calendar.set(Calendar.MINUTE, minute);
        calendar.set(Calendar.SECOND, 0);

        return calendar.getTime();
    }

}
