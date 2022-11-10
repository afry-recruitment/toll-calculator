package com.afry.tollfreedates;

import java.time.DayOfWeek;
import java.time.LocalDate;
import java.time.Month;
import java.util.List;

public class TollFreeDateCheck {

    public boolean isTollFreeDate(LocalDate date) {
        return isWeekend(date) || isJuly(date) || isHoliday(date);
    }

    private boolean isWeekend(LocalDate date) {
        return date.getDayOfWeek() == DayOfWeek.SATURDAY || date.getDayOfWeek() == DayOfWeek.SUNDAY;
    }

    private boolean isJuly(LocalDate date) {
        return date.getMonth() == Month.JULY;
    }

    private boolean isHoliday(LocalDate date) {
        List<LocalDate> holidays = new Holidays().getHolidays(date.getYear());

        for (LocalDate holiday : holidays) {
            if (date == holiday) return true;
        }
        return false;
    }
}
