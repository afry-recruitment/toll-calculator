package com.afry.timesandfees;

import com.afry.validator.CalculatorValidator;
import com.afry.vehicles.Vehicle;

import java.time.LocalDateTime;
import java.time.ZoneId;
import java.time.temporal.ChronoUnit;
import java.util.ArrayList;
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

            LocalDateTime intervalStart = passageTimes.get(0);

            int totalFee = 0;
            for (LocalDateTime date : passageTimes) {
                int nextFee = new Fee().calculate(date, vehicle);
                int tempFee = new Fee().calculate(intervalStart, vehicle);

                long minutes = ChronoUnit.MINUTES.between(intervalStart, date);

                if (minutes <= 60) {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                } else {
                    totalFee += nextFee;
                }
            }
            if (totalFee > TollTimesAndFees.maxTotal) totalFee = TollTimesAndFees.maxTotal;
            return totalFee;
        }
    }
}
