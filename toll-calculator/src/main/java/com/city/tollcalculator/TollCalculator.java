package com.city.tollcalculator;

import com.city.tollcalculator.utils.TimeRange;
import java.util.*;
import java.util.concurrent.TimeUnit;

public class TollCalculator {

    public int getTollFee(Vehicle vehicle, Date... dates) {
        if (isTollFreeVehicle(vehicle)) return 0;

        Arrays.sort(dates);
        Date intervalStart = dates[0];
        int totalFee = 0;

        for (Date date : dates) {
            int nextFee = getTollFee(date, vehicle);
            int tempFee = getTollFee(intervalStart, vehicle);

            long minutes = TimeUnit.MILLISECONDS.toMinutes(date.getTime() - intervalStart.getTime());

            if (minutes <= 60) {
                if (nextFee > tempFee) {
                    totalFee -= tempFee;
                    totalFee += nextFee;
                }
            } else {
                intervalStart = date;
                totalFee += nextFee;
            }

            if (totalFee >= TollRates.MAX_DAILY_FEE) {
                return TollRates.MAX_DAILY_FEE;
            }
        }
        return totalFee;
    }

    private boolean isTollFreeVehicle(Vehicle vehicle) {
        if (vehicle == null) return false;
        for (TollFreeVehicles tollFreeVehicle : TollFreeVehicles.values()) {
            if (tollFreeVehicle.getType().equals(vehicle.getType())) return true;
        }
        return false;
    }

    public int getTollFee(final Date date, Vehicle vehicle) {
        if (isTollFreeDate(date) || isTollFreeVehicle(vehicle)) return 0;

        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);
        int hour = calendar.get(Calendar.HOUR_OF_DAY);
        int minute = calendar.get(Calendar.MINUTE);

        for (Map.Entry<TimeRange, Integer> entry : TollRates.TIME_TO_FEE_MAP.entrySet()) {
            if (entry.getKey().isInRange(hour, minute)) {
                return entry.getValue();
            }
        }
        return 0;
    }

    private boolean isTollFreeDate(Date date) {
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);
        int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
        return dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY;
    }
}
