package com.afry.validator;

import com.afry.vehicles.*;

import java.time.LocalDate;
import java.time.ZoneId;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class CalculatorValidator {
    public static String validate(Vehicle vehicle, Date... dates) {
        if (vehicle == null) {
            return "Vehicle can not be null!";
        }
        if (dates == null) {
            return "Date can not be null!";
        }

        return isAllDateSameDay(dates);
    }

    private static String isAllDateSameDay(Date... dates) {
        List<LocalDate> passageDates = new ArrayList<>();
        for (Date date : dates) {
            passageDates.add(date.toInstant().atZone(ZoneId.systemDefault()).toLocalDate());
        }
        if (passageDates.size() == 2) {
            if (!passageDates.get(0).equals(passageDates.get(1)))
                return "All times must be on same day!";
        }
        if (passageDates.size() > 2) {
            for (int i = 0; i < passageDates.size(); i++) {
                for (int j = i + 1; j < passageDates.size(); j++) {
                    if (!passageDates.get(i).equals(passageDates.get(j))) {
                        return "All times must be on same day!";
                    }
                }
            }
        }
        return null;
    }
}
