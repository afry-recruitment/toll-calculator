package com.afry.timesandfees;

import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;

public class Fee {
    public int getFee(LocalTime time) {

        int hour = time.getHour();
        int minute = time.getMinute();

        List<TollTimeSlot> timeSlots = new ArrayList<>(new TollTimesAndFees().timesAndFees());

        for (TollTimeSlot slot : timeSlots) {
            if ((hour >= slot.startHour() && minute >= slot.startMinute())
                    && (hour <= slot.endHour() && minute <= slot.endMinute()))
                return slot.fee();
        }
        return 0;
    }
}