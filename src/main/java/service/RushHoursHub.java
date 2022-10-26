package service;

import model.Vehicle;

import java.time.LocalDateTime;
import java.util.Calendar;
import java.util.HashMap;

import static config.TollConfiguration.getTollFeeAtPeakTimes;
import static service.TollCalculator.localDateTimeToCalendar;


public class RushHoursHub {


    public int getTollFeeAtPeakTimesCalculus(final LocalDateTime date, Vehicle vehicle) {

        TollCalculator tollCalculator = new TollCalculator();

        if(Boolean.TRUE.equals(tollCalculator.isTollFreeDate(date)) || tollCalculator.isTollFreeVehicle(vehicle)) return 0;
        Calendar calendar = Calendar.getInstance();
        calendar.setTime(localDateTimeToCalendar(date));
        int hour = calendar.get(Calendar.HOUR_OF_DAY);
        int minute = calendar.get(Calendar.MINUTE);

        int hourMinute = Integer.parseInt(String.format("%d%d", hour, minute));
        if(hourMinute < 100) {
            hourMinute = hour * 100;
        }
        int finalResult = 0;

        for (HashMap<String, Integer> option : getTollFeeAtPeakTimes()) {
            int from  = option.get("from");
            int to  = option.get("to");
            int result  = option.get("result");

            if (hourMinute >= from && hourMinute <= to) {
                finalResult = result;
                break;
            }
        }
        return finalResult;

    }

}
