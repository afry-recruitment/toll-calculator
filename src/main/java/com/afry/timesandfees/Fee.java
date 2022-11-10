package com.afry.timesandfees;

import com.afry.tollfreedates.TollFreeDateCheck;
import com.afry.vehicles.Vehicle;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;

public class Fee {
    public int calculate(final LocalDateTime passageDateTime, Vehicle vehicle) {
        LocalDate passageDate = passageDateTime.toLocalDate();
        boolean isTollFreeDate = new TollFreeDateCheck().isTollFreeDate(passageDate);

        if (isTollFreeDate || vehicle.isTollFree()) return 0;

        LocalTime passageTime = passageDateTime.toLocalTime();

        return getFee(passageTime);
    }

    public int getFee(LocalTime time) {

        List<TollTimeSlot> timeSlots = new ArrayList<>(new TollTimesAndFees().timesAndFees());

        for (TollTimeSlot slot : timeSlots) {
            if (!time.isBefore(slot.startTime()) && time.isBefore(slot.endTime()))
                return slot.fee();
        }
        return 0;
    }
}