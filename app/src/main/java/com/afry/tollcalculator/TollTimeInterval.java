package com.afry.tollcalculator;

public record TollTimeInterval(int startHour, int startMinute, int endHour, int endMinute) {
    public boolean isInside(int hour, int minute) {
        int minuteInDay = hour * 60 + minute;
        return (minuteInDay >= startHour * 60 + startMinute) && (minuteInDay <= endHour * 60 + endMinute);
    }
}
