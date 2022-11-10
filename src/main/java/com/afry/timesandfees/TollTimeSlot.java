package com.afry.timesandfees;

import java.time.LocalTime;

public record TollTimeSlot(LocalTime startTime, LocalTime endTime, int fee) {
}
