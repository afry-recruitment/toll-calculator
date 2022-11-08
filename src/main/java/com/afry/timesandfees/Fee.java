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

        int hour = time.getHour();
        int minute = time.getMinute();

        List<TollTimeSlot> timeSlots = new ArrayList<>(new TollTimesAndFees().timesAndFees());

        for (TollTimeSlot slot : timeSlots) {
            if ((hour == slot.startHour() && minute >= slot.startMinute()) && minute <= slot.endMinute())
                return slot.fee();
            else if (hour > slot.startHour() && (hour <= slot.endHour() && minute <= slot.endMinute()))
                return slot.fee();
        }
        return 0;
    }
}