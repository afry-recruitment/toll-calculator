package com.afry.service;

import com.afry.model.Vehicle;

import java.time.Duration;
import java.time.LocalDateTime;
import java.time.LocalTime;

public class TollCalculatorService {

    public static final int MAX_TOLL_FEE_PER_DAY = 60;
    int totalFee = 0;

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public int getTollFee(Vehicle vehicle, LocalDateTime[] dates) {
        LocalDateTime previousTollTime = null;
        int previousFee = 0;
        LocalDateTime tollCollectedDay = dates[0];
        TollFreeDayService tollFreeDayService = new TollFreeDayService();

        if (vehicle.isTollFreeVehicle() || tollFreeDayService.isTollFreeDay(tollCollectedDay)) {
            return totalFee;
        } else {
            for (LocalDateTime date : dates) {
                int fee = getFee(date);
                if (previousTollTime != null) {
                    Duration duration = Duration.between(previousTollTime, date);
                    long minutesDifference = duration.toMinutes();

                    if (minutesDifference <= 60 && previousFee <= fee) {
                        totalFee = totalFee - previousFee + fee;
                    } else {
                        totalFee += fee;
                    }
                    if (totalFee > MAX_TOLL_FEE_PER_DAY) {
                        totalFee = MAX_TOLL_FEE_PER_DAY;
                        break;
                    }
                } else {
                    totalFee = fee;
                }
                previousTollTime = date;
                previousFee = fee;
            }
        }
        return totalFee;
    }

    /**
     * Calculate the total toll fee for one day
     *
     * @param dateTime - the datetime during passing of the toll
     * @return - the total toll fee for the datetime
     */
    public int getFee(LocalDateTime dateTime) {
        LocalTime time = dateTime.toLocalTime();

        if (isInRange(time, LocalTime.of(7, 0, 0), LocalTime.of(8, 29, 59)) ||
                isInRange(time, LocalTime.of(15, 30, 0), LocalTime.of(17, 29, 59))) {
            return 18;
        } else if (isInRange(time, LocalTime.of(12, 0, 0), LocalTime.of(13, 59, 59)) ||
                isInRange(time, LocalTime.of(17, 30, 0), LocalTime.of(18, 59, 59))) {
            return 13;
        } else {
            return 8;
        }
    }

    /**
     *To check if the requested time is within a given range of time
     *
     * @param requestTime - the time during passing of the toll
     * @param beginTime - the start time of the range
     * @param endTime - the end time of the range
     * @return - boolean value if the requestTime is withing the inclusive range
     */
    public boolean isInRange(LocalTime requestTime, LocalTime beginTime, LocalTime endTime) {
        return !requestTime.isBefore(beginTime) && !requestTime.isAfter(endTime);
    }
}

