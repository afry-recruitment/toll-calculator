package com.assignment.toll_calculator.services;

import com.assignment.toll_calculator.errors.ApiException;
import com.assignment.toll_calculator.models.internal.VehicleType;
import com.assignment.toll_calculator.utils.DateUtils;
import com.assignment.toll_calculator.utils.FileUtils;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.stereotype.Service;

import java.util.*;

import static com.assignment.toll_calculator.utils.ConstantUtils.*;

/**
 * This class is responsible for calculating the toll fee.
 */
@Service
@Slf4j
public class TollService {

    /**
     * Calculate the total toll fee response for all dates on one day
     *
     * @param vehicleType - the vehicle type
     * @param dates   - date and time of all passes on one day
     * @return - the total toll response for that day
     */
    public Integer calculateFee(VehicleType vehicleType, List<Date> dates) {
        if (vehicleType == null) throw new ApiException(INVALID_VEHICLE_TYPE_ERROR_MESSAGE);
        if (dates == null) throw new ApiException(INVALID_DATES_ERROR_MESSAGE);

        if (dates.isEmpty()) {
            log.warn("The dates are empty!");
            return 0;
        }

        dates = new ArrayList<>(dates.stream().sorted().toList());

        Date intervalStart = dates.getFirst();

        if (vehicleType.isTollFree()) {
            log.debug("The vehicle type {} is toll free!", vehicleType);
            return 0;
        }
        if (isTollFreeDate(intervalStart)) {
            log.debug("The date {} is toll free!", intervalStart);
            return 0;
        }

        boolean isDifferentDays = dates.stream().anyMatch(date -> !isSameDay(date, intervalStart));
        if (isDifferentDays) {
            throw new ApiException(DIFFERENT_DAYS_ERROR_MESSAGE, HttpStatus.BAD_REQUEST);
        }

        int totalFee = calculateFee(dates);

        log.debug("Calculated total fee {} for vehicle type {} and dates {}", totalFee, vehicleType, dates);

        return totalFee;
    }

    /**
     * Calculate the toll fee for list of dates
     *
     * @param dates - the list of dates
     * @return - the toll fee for that list of dates
     */
    private int calculateFee(List<Date> dates) {
        int totalFee = 0;
        Date referenceDate = dates.getFirst();
        int referenceFee = calculateFee(referenceDate);

        if (dates.size() == 1) {
            return referenceFee;
        }

        for (int dateIndex = 1; dateIndex < dates.size(); dateIndex++) {
            Date currentDate = dates.get(dateIndex);
            int currentFee = calculateFee(currentDate);

            long diffInMillis = currentDate.getTime() - referenceDate.getTime();
            double minutes = (double) diffInMillis / 1000 / 60;

            if (minutes <= 60D) {
                referenceFee = Math.max(referenceFee, currentFee);
            } else {
                totalFee += referenceFee;
                referenceFee = currentFee;
                referenceDate = currentDate;
            }

            if (dateIndex == dates.size() - 1) {
                totalFee += referenceFee;
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

        List<Date> holidays = FileUtils.loadAsList(HOLIDAYS_FILE_PATH)
                .stream()
                .map(DateUtils::convert)
                .toList();

        return holidays.stream().anyMatch(holiday -> isSameDay(holiday, date));
    }

}
