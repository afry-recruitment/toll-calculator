package com.afry.timesandfees;

public record TollTimeSlot(int startHour, int startMinute, int endHour, int endMinute, int fee) {
}
