package com.city.tollcalculator;

public class TimeRange {
    private final int startHour;
    private final int startMinute;
    private final int endHour;
    private final int endMinute;

    public TimeRange(int startHour, int startMinute, int endHour, int endMinute) {
        this.startHour = startHour;
        this.startMinute = startMinute;
        this.endHour = endHour;
        this.endMinute = endMinute;
    }

    public boolean isInRange(int hour, int minute) {
        if (hour < startHour || hour > endHour) return false;
        if (hour == startHour && minute < startMinute) return false;
        if (hour == endHour && minute > endMinute) return false;
        return true;
    }
}
