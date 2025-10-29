package com.assignment.toll_calculator.services;

import com.assignment.toll_calculator.models.Vehicle;
import com.assignment.toll_calculator.utils.DateUtils;
import com.assignment.toll_calculator.utils.FileUtils;

import java.util.*;
import java.util.concurrent.*;

/**
 * This class is responsible for calculating the toll fee.
 */
public class TollService {

    public static final String INVALID_VEHICLE_ERROR_MESSAGE =
            "Cannot identify toll for null vehicle!";
    public static final String INVALID_DATES_ERROR_MESSAGE =
            "Cannot identify toll for null dates!";
    public static final String DIFFERENT_DAYS_ERROR_MESSAGE =
            "Cannot identify tolls for dates from different days at the same time!";

    private static final Integer MAX_TOTAL_FEE = 60;

    /**
     * Calculate the total toll fee for all dates on one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public int calculateFee(Vehicle vehicle, List<Date> dates) {
        if (vehicle == null) throw new IllegalArgumentException(INVALID_VEHICLE_ERROR_MESSAGE);
        if (dates == null) throw new IllegalArgumentException(INVALID_DATES_ERROR_MESSAGE);

        if (dates.isEmpty()) return 0;

        dates = new ArrayList<>(dates.stream().sorted().toList());

        Date intervalStart = dates.getFirst();

        if (vehicle.isTollFree() || isTollFreeDate(intervalStart)) return 0;

        if (dates.size() == 1) return calculateFee(intervalStart);

        boolean isDifferentDays = dates.stream().anyMatch(date -> !isSameDay(date, intervalStart));
        if (isDifferentDays) {
            throw new IllegalArgumentException(DIFFERENT_DAYS_ERROR_MESSAGE);
        }

        return calculateFee(dates);

    }

    /**
     * Calculate the toll fee for list of dates
     *
     * @param dates - the list of dates
     * @return - the toll fee for that list of dates
     */
    private int calculateFee(List<Date> dates) {
        int totalFee = 0;

        for (int dateIndex = 0; dateIndex < dates.size(); dateIndex++) {
            Date actualDate = dates.get(dateIndex);
            int actualFee = calculateFee(actualDate);

            if (dateIndex == 0) {
                totalFee += actualFee;
                continue;
            }

            Date previousDate = dates.get(dateIndex - 1);
            int previousFee = calculateFee(previousDate);

            long diffInMillis = actualDate.getTime() - previousDate.getTime();
            double minutes = (double) diffInMillis / 1000 / 60;

            if (minutes <= 60D) {
                if (totalFee > 0) totalFee -= previousFee;
                totalFee += Math.max(actualFee, previousFee);
            } else {
                totalFee += actualFee;
            }

            if (totalFee >= MAX_TOTAL_FEE) return MAX_TOTAL_FEE;
        }

        return totalFee;
    }

    /**
     * Calculate the toll fee for one date
     *
     * @param date - the date
     * @return - the toll fee for that date
     */
    private int calculateFee(Date date) {
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);
        int hour = calendar.get(Calendar.HOUR_OF_DAY);
        int minute = calendar.get(Calendar.MINUTE);

        if (hour == 6 && minute <= 29) return 8;
        else if (hour == 6) return 13;
        else if (hour == 7) return 18;
        else if (hour == 8 && minute <= 29) return 13;
        else if (hour >= 8 && hour <= 14) return 8;
        else if (hour == 15 && minute <= 29) return 13;
        else if (hour == 15 || hour == 16) return 18;
        else if (hour == 17) return 13;
        else if (hour == 18 && minute <= 29) return 8;
        else return 0;
    }

    /**
     * Check if two dates are from the same day or not
     *
     * @param dateOne - one of the dates
     * @param dateTwo - the other date
     * @return - true if the two dates are from the same day, otherwise false
     */
    private boolean isSameDay(Date dateOne, Date dateTwo) {
        Calendar calendarOne = Calendar.getInstance();
        calendarOne.setTime(dateOne);

        Calendar calendarTwo = Calendar.getInstance();
        calendarTwo.setTime(dateTwo);

        return calendarOne.get(Calendar.YEAR) == calendarTwo.get(Calendar.YEAR)
                && calendarOne.get(Calendar.MONTH) == calendarTwo.get(Calendar.MONTH)
                && calendarOne.get(Calendar.DAY_OF_MONTH) == calendarTwo.get(Calendar.DAY_OF_MONTH);
    }

    /**
     * Check if a date is toll-free or not
     *
     * @param date - the date
     * @return - true if the date is toll-free, otherwise false
     */
    private Boolean isTollFreeDate(Date date) {
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);

        int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
        if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) return true;

        List<Date> holidays = FileUtils.loadAsList(FileUtils.HOLIDAYS_FILE_PATH)
                .stream()
                .map(DateUtils::convert)
                .toList();

        return holidays.stream().anyMatch(holiday -> isSameDay(holiday, date));
    }

}
