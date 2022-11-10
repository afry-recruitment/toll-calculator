package com.afry.timesandfees;

import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class TollTimesAndFees {
    public static final int maxTotal = 60;
    private final List<TollTimeSlot> tollTimesAndFeesList = new ArrayList<>();

    public List<TollTimeSlot> timesAndFees() {
        int lowTrafficPrice = 8;
        int midTrafficPrice = 13;
        int rushHourPrice = 18;

        tollTimesAndFeesList.addAll(Arrays.asList(
                (new TollTimeSlot(LocalTime.of(6, 0), LocalTime.of(6, 30),lowTrafficPrice)),
                (new TollTimeSlot(LocalTime.of(6, 30), LocalTime.of(7, 0), midTrafficPrice)),
                (new TollTimeSlot(LocalTime.of(7, 0), LocalTime.of(8, 0), rushHourPrice)),
                (new TollTimeSlot(LocalTime.of(8, 0), LocalTime.of(8, 30), midTrafficPrice)),
                (new TollTimeSlot(LocalTime.of(8, 30), LocalTime.of(15, 0), lowTrafficPrice)),
                (new TollTimeSlot(LocalTime.of(15, 0), LocalTime.of(15, 30), midTrafficPrice)),

                //Rush hour price below beginning at 15.30. Assumed mistake when starting at 15.00.
                (new TollTimeSlot(LocalTime.of(15, 30), LocalTime.of(17, 0), rushHourPrice)),
                (new TollTimeSlot(LocalTime.of(17, 0), LocalTime.of(18, 0), midTrafficPrice)),
                (new TollTimeSlot(LocalTime.of(18, 0), LocalTime.of(18, 30), lowTrafficPrice))
        ));

        return tollTimesAndFeesList;
    }
}
