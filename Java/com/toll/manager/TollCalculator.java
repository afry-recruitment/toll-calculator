package com.toll.manager;

import com.toll.model.Vehicle;
import com.toll.util.FeeCalculatorUtil;

import java.io.IOException;
import java.time.LocalDateTime;
import java.util.*;

public class TollCalculator {

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle here in test class is car which is not toll free
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    private int year;

    /**
     * The constructor of this class.
     *
     * @param year The year for which fees will be calculated from. Depending on  years have different dates for holidays and .txt file need to be updated accordingly.
     */
    public TollCalculator(int year) {
        this.year = year;

    }

    /**
     * Calculate the total toll fee for one day <- this means that all the dates are
     * the same, only time differs
     *
     * @param vehicle   - the Car(Tolled)
     * @param vehicle   - the Emergency or other toll-free vehicle(Toll free) can be used in test class
     * @param dateTimes - All the date and time of all passes on one day, using LocalDateTime format. Should be sorted in proper order
     * @return - the total highest toll fee for that day for the given vehicle not exceeding 60 SEK
     */
    public int getTotalDailyFee(Vehicle vehicle, LocalDateTime... dateTimes) throws IOException {

        int totalFee = 0;
        List<Integer[][]> range = new ArrayList<>();
        for (LocalDateTime date : dateTimes) {
            int startRange = date.getHour() * 60 + date.getMinute();
            int endRange = date.getHour() * 60 + date.getMinute() + 60;
            Integer[] partRange = {startRange, endRange};
            Integer fee = FeeCalculatorUtil.getFee(vehicle, date, year);
            Integer[][] feeForPeriod = {partRange, {fee}};

            boolean concurrent = false;
            int sizes = range.size();

            ListIterator<Integer[][]> reservedTimeRanges = range.listIterator(sizes);
            while (reservedTimeRanges.hasPrevious()) {

                Integer[][] occupiedRange = reservedTimeRanges.previous();
                if (startRange > occupiedRange[0][1])
                    break;

                if (startRange >= occupiedRange[0][0] && startRange <= occupiedRange[0][1]) {

                    if (fee > occupiedRange[1][0]) {
                        occupiedRange[1][0] = fee;
                    }


                    concurrent = true;
                    break;
                }
                sizes--;
            }


            if (!concurrent) {
                range.add(feeForPeriod);
                concurrent = true;
            }

        }

        for (Integer[][] partRange : range) {
            totalFee += partRange[1][0];
        }
        if (totalFee > 60)
            totalFee = 60;

        return totalFee;
    }


}



