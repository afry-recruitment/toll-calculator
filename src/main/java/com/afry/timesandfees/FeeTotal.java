package com.afry.timesandfees;

import com.afry.validator.CalculatorValidator;
import com.afry.vehicles.Vehicle;

import java.time.LocalDateTime;
import java.time.ZoneId;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.List;

public class FeeTotal {
    public int calculate(Vehicle vehicle, Date... dates) {
        String error = CalculatorValidator.validate(vehicle, dates);
        if (error != null) {
            System.out.println(error);
            return -1;
        } else {
            // convert dates to List of LocalDateTime
            List<LocalDateTime> passageTimes = new ArrayList<>();
            for (Date date : dates) {
                passageTimes.add(date.toInstant().atZone(ZoneId.systemDefault()).toLocalDateTime());
            }

            if (passageTimes.size() == 1) return new Fee().calculate(passageTimes.get(0), vehicle);

            // Sort passage times. Chronology important when checking time between passages!
            Collections.sort(passageTimes);

            int totalFee = 0;
            for (int i = 0; i < passageTimes.size(); i++) {
                int nextFee = new Fee().calculate(passageTimes.get(i), vehicle);
                for (int j = i + 1; j < passageTimes.size(); j++) {
                    long minutes = ChronoUnit.MINUTES.between(passageTimes.get(i), passageTimes.get(j));
                    if (minutes < 60) {
                        int tempFee = new Fee().calculate(passageTimes.get(j), vehicle);
                        if (tempFee > nextFee) nextFee = tempFee;
                        i++;
                    }
                }
                totalFee += nextFee;
            }

            if (totalFee > TollTimesAndFees.maxTotal) totalFee = TollTimesAndFees.maxTotal;
            return totalFee;
        }
    }
}
