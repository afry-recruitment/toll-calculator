package com.toll.util;

import com.toll.model.Vehicle;

import java.io.IOException;
import java.time.LocalDateTime;

public class FeeCalculatorUtil {
    private FeeCalculatorUtil() {
    }

    /**
     * @param vehicle  The vehicle in question
     * @param dateTime The date and time of registered fee
     * @return Returns an int representing the fee the vehicle will receive based on the dateTime
     */
    public static Integer getFee(Vehicle vehicle, final LocalDateTime dateTime, int year) throws IOException {
        if (TollFreeCalculatorUtil.isTollFreeVehicle(vehicle) || GetHolidayDateUtil.isTollFreeHolidayDate(dateTime, year))
            return 0;
        int hour = dateTime.getHour();
        int minute = dateTime.getMinute();

        if (hour == 6 && minute >= 0 && minute <= 29)
            return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59)
            return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59)
            return 18;
        else if (hour >= 8 && hour <= 14) {
            if ((hour == 8) && (minute <= 29))
                return 13;
            return 8;
        } else if (hour == 15 && minute >= 0 && minute <= 29)
            return 13;
        else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59)
            return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59)
            return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29)
            return 8;
        else
            return 0;

    }
}
