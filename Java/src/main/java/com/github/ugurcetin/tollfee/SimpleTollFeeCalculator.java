package com.github.ugurcetin.tollfee;

import java.util.*;

public class SimpleTollFeeCalculator implements TollFeeCalculator {

    private final List<Holiday> holidays;
    private final List<TollFeePeriod> periodList;

    public SimpleTollFeeCalculator(List<TollFeePeriod> periodList, List<Holiday> holidays) {
        this.periodList = periodList;
        this.holidays = holidays;
    }

    public int calculateTollFee(Vehicle vehicle, Date date) {
        if (Objects.isNull(vehicle) || Objects.isNull(date) || isHoliday(date) || vehicle.isFeeFree()) {
            return 0;
        }

        final Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);
        final int hour = calendar.get(Calendar.HOUR_OF_DAY);
        final int minute = calendar.get(Calendar.MINUTE);

        Optional<TollFeePeriod> tollFeePeriod = periodList.stream()
                .filter(period -> hour >= period.getStartHour() && hour <= period.getEndHour() && minute >= period.getStartMinute() && minute <= period.getEndMinute())
                .findFirst();

        return tollFeePeriod.map(TollFeePeriod::getFee).orElse(0);
    }

    public int calculateTollFee(Vehicle vehicle, List<Date> dates) {
        if (Objects.isNull(vehicle) || Objects.isNull(dates) || vehicle.isFeeFree()) {
            return 0;
        }

        // hour-fee
        // 1->10
        final Map<Integer, Integer> feeMap = new HashMap<>();
        final Calendar calendar = GregorianCalendar.getInstance();

        dates.forEach(date -> {
            calendar.setTime(date);
            int hour = calendar.get(Calendar.HOUR_OF_DAY);
            int fee = calculateTollFee(vehicle, date);
            if (!feeMap.containsKey(hour) || feeMap.get(hour) < fee) {
                feeMap.put(hour, fee);
            }
        });

        int totalFee = feeMap.values().stream().reduce(0, Integer::sum);
        return Math.min(totalFee, 60);
    }

    private boolean isHoliday(Date date) {
        if (holidays == null) {
            return false;
        }

        final Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);

        final int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
        if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) {
            return true;
        }

        final int year = calendar.get(Calendar.YEAR);
        final int month = calendar.get(Calendar.MONTH);
        final int day = calendar.get(Calendar.DAY_OF_MONTH);

        return holidays.stream().anyMatch(holiday -> holiday.getYear() == year && holiday.getMonth() == month && holiday.getDay() == day);
    }

}
