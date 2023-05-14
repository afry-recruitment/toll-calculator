package com.tollcalculator.util;

import com.tollcalculator.model.City;
import com.tollcalculator.model.CityHolidayCalendar;
import com.tollcalculator.model.CityHolidayMonthCalendar;
import com.tollcalculator.model.CityWorkingCalendar;
import org.apache.commons.lang3.time.DateUtils;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.*;

public class DateTimeUtil {
    public static final SimpleDateFormat dateAndTimeFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

    public static Boolean getWeekendFromCalendar(CityWorkingCalendar cityWorkingCalendar,int day){
        if(cityWorkingCalendar == null)
            return false;
        if(cityWorkingCalendar.isMonday()==false && day == Calendar.MONDAY) return true;
        if(cityWorkingCalendar.isTuesday()==false && day == Calendar.TUESDAY) return true;
        if(cityWorkingCalendar.isWednesday()==false && day == Calendar.WEDNESDAY) return true;
        if(cityWorkingCalendar.isThursday()==false && day == Calendar.THURSDAY) return true;
        if(cityWorkingCalendar.isFriday()==false && day == Calendar.FRIDAY) return true;
        if(cityWorkingCalendar.isSaturday()==false && day == Calendar.SATURDAY) return true;
        if(cityWorkingCalendar.isSunday()==false && day == Calendar.SUNDAY) return true;
        return false;
    }

    public static Boolean getCityHolidayMonthFromCalendar(CityHolidayMonthCalendar cityHolidayMonthCalendar, int month) {
        if(cityHolidayMonthCalendar == null) return false;

        if(cityHolidayMonthCalendar.isJanuary() == true && month == (Calendar.JANUARY)) return true;
        if(cityHolidayMonthCalendar.isFebruary() == true && month == (Calendar.FEBRUARY)) return true;
        if(cityHolidayMonthCalendar.isMarch() == true && month == (Calendar.MARCH)) return true;
        if(cityHolidayMonthCalendar.isApril() == true && month == (Calendar.APRIL)) return true;
        if(cityHolidayMonthCalendar.isMay() == true && month == (Calendar.MAY)) return true;
        if(cityHolidayMonthCalendar.isJune() == true && month == (Calendar.JUNE)) return true;
        if(cityHolidayMonthCalendar.isJuly() == true && month == (Calendar.JULY)) return true;
        if(cityHolidayMonthCalendar.isAugust() == true && month == (Calendar.AUGUST)) return true;
        if(cityHolidayMonthCalendar.isSeptember() == true && month == (Calendar.SEPTEMBER)) return true;
        if(cityHolidayMonthCalendar.isOctober() == true && month == (Calendar.OCTOBER)) return true;
        if(cityHolidayMonthCalendar.isNovember() == true && month == (Calendar.NOVEMBER)) return true;
        if(cityHolidayMonthCalendar.isDecember() == true && month == (Calendar.DECEMBER)) return true;

        return false;
    }

    public static boolean getDateBeforeHolidayORPublicHoliday(Date date, City city){
        Set<CityHolidayCalendar> cityPublicHoliday = city.getCityHolidayCalenders();
        if(cityPublicHoliday == null || cityPublicHoliday.isEmpty()) return true;

        for(CityHolidayCalendar cityHolidayCalendar : cityPublicHoliday){
            if(DateUtils.isSameDay(cityHolidayCalendar.getDate(),date)){
                System.out.println("public holiday");
                return true;
            }
        }
        for(CityHolidayCalendar cityHolidayCalendar : cityPublicHoliday) {
            Integer numberOfTaxFreeDaysBeforeHoliday = city.getCityTaxChoice().getNumberOfTaxFreeDaysBeforeHoliday();
            Date min = DateUtils.addDays(cityHolidayCalendar.getDate(), numberOfTaxFreeDaysBeforeHoliday);
            if(isDateBeforeHoliday(min, cityHolidayCalendar.getDate(), date)) {
                System.out.println("date before public holiday");
                return true;
            }
        }
        return false;
    }

    public static boolean isDateBeforeHoliday(final Date min, final Date max, final Date date){
        return !(date.before(min) || date.after(max));
    }

    public static void sortDateByAsc(List<Date> dates) {
        Collections.sort(dates, new Comparator<Date>() {
            @Override
            public int compare(Date object1, Date object2) {
                return object1.compareTo(object2);
            }
        });
    }

    public static Date formatToDate(Object date) throws ParseException {
        if(date instanceof String)
            return dateAndTimeFormat.parse(date.toString());
        else
            return dateAndTimeFormat.parse(dateAndTimeFormat.format(((Date)date)));
    }
}
