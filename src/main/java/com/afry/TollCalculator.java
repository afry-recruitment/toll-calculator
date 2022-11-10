package com.afry;

import java.time.*;
import java.util.*;

import com.afry.vehicles.*;
import com.afry.timesandfees.*;

public class TollCalculator {
    /*
        Well, who doesn't love bonus points? The clip in the readme file is from the 1995 classic "Hackers".
        I've actually seen the film through an ex-girlfriend when I was in high school. Sorry to say I
        didn't recognize the clip from memory, but a simple google search of the phrase
        "Hack the planet" gave me the answer. Where would we be without Google though?
     */
    public static void main(String[] args) {
        Vehicle vehicle = new Car();
        LocalDateTime ldt = LocalDateTime.of(2022, 11, 7, 16, 33);
        Date date = java.util.Date.from(ldt.atZone(ZoneId.systemDefault()).toInstant());

        int fee = new FeeTotal().calculate(vehicle, date);

        System.out.println(fee);
    }
    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
}

