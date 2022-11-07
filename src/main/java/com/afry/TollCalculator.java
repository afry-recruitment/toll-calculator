package com.afry;

import java.time.*;
import java.time.temporal.ChronoUnit;
import java.util.*;

import com.afry.vehicles.*;
import com.afry.tollfreedates.*;
import com.afry.timesandfees.*;
import com.afry.validator.CalculatorValidator;

public class TollCalculator {

    /*
        Well, who doesn't love bonus points? The clip in the readme file is from the 1995 classic "Hackers".
        I've actually seen the film through an ex-girlfriend when I was in high school. However,
        I must admit I didn't recognize the clip from memory. A simple google search of the phrase
        "Hack the planet" gave me the answer. Where would we be without Google though?
     */
    public static void main(String[] args) {
        Vehicle vehicle = new Car();
        LocalDateTime ldt = LocalDateTime.of(2022,11,7,16,33);
        Date date = java.util.Date.from(ldt.atZone(ZoneId.systemDefault()).toInstant());

        int fee = getTotalFee(vehicle, date);

        System.out.println(fee);
    }

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public static int getTotalFee(Vehicle vehicle, Date... dates) {
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
                int nextFee = getTollFee(date, vehicle);
                int tempFee = getTollFee(intervalStart, vehicle);

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


    public static int getTollFee(final LocalDateTime passageDateTime, Vehicle vehicle) {
        LocalDate passageDate = passageDateTime.toLocalDate();
        boolean isTollFreeDate = new TollFreeDateCheck().isTollFreeDate(passageDate);

        if (isTollFreeDate || vehicle.isTollFree()) return 0;

        LocalTime passageTime = passageDateTime.toLocalTime();

        return new Fee().getFee(passageTime);
    }


}

