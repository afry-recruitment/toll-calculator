package com.assignment.toll_calculator.utils;

import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

public class DateUtils {

    public static Date composeDate(int month, int day, int hour, int minute) {
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.set(2013, month, day, hour, minute);
        return calendar.getTime();
    }

    public static Date composeDate(int month, int day) {
        return composeDate(month, day, 0, 0);
    }

}
