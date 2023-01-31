package com.toll.calculator;

import com.toll.calculator.exception.DateException;
import com.toll.calculator.vehicle.Vehicle;
import org.joda.time.DateTime;

import java.time.LocalDate;
import java.time.LocalTime;
import java.util.*;

// NOTE: Changed all methods and enum accessors to public for JUnit tests' accessibility
public class TollCalculator {
    public static final int DAILY_MAX_FEE = 60;
    public static final int CHARGE_PERIOD_IN_MINUTES = 60;
    public static final Map<LocalTime, TollFee> TOLL_FEE_MAP = getTollFeeMap();

    private final DateHandler dateHandler;

    public enum TollFee {
        FREE(0),
        LOW(8),
        MID(13),
        HIGH(18);

        private final int fee;

        TollFee(int fee) {
            this.fee = fee;
        }

        public int getFee() {
            return fee;
        }
    }

    public TollCalculator(DateHandler dateHandler) {
        this.dateHandler = dateHandler;
    }

    public static Map<LocalTime, TollFee> getTollFeeMap() {
        //Using linked hashmap to preserving the order on retrievals
        Map<LocalTime, TollFee> tollFeeMap = new LinkedHashMap<>();

        // Insert into asc order - Inclusive to exclusive next time in future, e.g. 06:00 - 06:29 low fee
        tollFeeMap.put(LocalTime.of(0,0), TollFee.FREE);
        tollFeeMap.put(LocalTime.of(6,0), TollFee.LOW);
        tollFeeMap.put(LocalTime.of(6,30), TollFee.MID);
        tollFeeMap.put(LocalTime.of(7,0), TollFee.HIGH);
        tollFeeMap.put(LocalTime.of(8,0), TollFee.MID);
        tollFeeMap.put(LocalTime.of(8,30), TollFee.LOW);
        tollFeeMap.put(LocalTime.of(15,0), TollFee.MID);
        tollFeeMap.put(LocalTime.of(15,30), TollFee.HIGH);
        tollFeeMap.put(LocalTime.of(17,0), TollFee.MID);
        tollFeeMap.put(LocalTime.of(18,0), TollFee.LOW);
        tollFeeMap.put(LocalTime.of(18,30), TollFee.FREE);

        return Collections.unmodifiableMap(tollFeeMap);
    }

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public int getTollFee(Vehicle vehicle, Date... dates) throws DateException {
        if (vehicle == null) {
            throw new NullPointerException("Vehicle cannot be null!");
        }
        if (dates == null) {
            throw new NullPointerException("Dates cannot be null!");
        }
        if (dates.length == 0) {
            throw new DateException("At least one date must be given!");
        }
        //Creating arraylist of array and sorting in ascending order for easier calculation
        ArrayList<Date> dateList = new ArrayList<>(Arrays.asList(dates));
        Collections.sort(dateList);

        // Only allow dates to be for same day - to conform with javadocs above
        if (!Utils.isDatesWithinSameDay(dateList)) {
            throw new DateException("The given dates must all be for one day!");
        }

        // Base case - if vehicle toll-free or the day is toll-free
        if (!isTollable(vehicle, dateList.get(0))) {
            return 0;
        }
        // Creating partitions of intersecting dates based on the charge period
        List<List<Toll>> intersectingDateCharges = getIntersectingDateCharges(vehicle, dateList);

        // Calculate the highest charge in each partition and sum them all up
        int totalTollFee = 0;
        for (List<Toll> partition : intersectingDateCharges) {
            OptionalInt max = partition
                    .stream()
                    .mapToInt(t -> t.getFee().getFee())
                    .max();
            if (max.isPresent()) {
                totalTollFee += max.getAsInt();
            }
        }
        return Math.min(totalTollFee, DAILY_MAX_FEE);
    }


    private Toll createToll(Vehicle vehicle, DateTime dateTime) {
        LocalTime localTime = DateHandler.getLocalTimeFromDate(dateTime.toDate());
        TollFee tollFee = getTollFeeForTime(localTime);
        return new Toll(vehicle, dateTime, tollFee);
    }

    private void createAndAddPartition(List<List<Toll>> intersectingDateCharges, Toll toll) {
        List<Toll> chargePartition = new ArrayList<>();
        chargePartition.add(toll);
        intersectingDateCharges.add(chargePartition);
    }

    // Expecting a dateList in asc order
    public List<List<Toll>> getIntersectingDateCharges(Vehicle vehicle, List<Date> dateList) {
        List<List<Toll>> intersectingDateCharges = new ArrayList<>();
        int partitions = 0;

        for (Date date : dateList) {
            DateTime dateTime = new DateTime(date);
            Toll toll = createToll(vehicle, dateTime);
            if (intersectingDateCharges.isEmpty()) {
                createAndAddPartition(intersectingDateCharges, toll);
                partitions++;
            } else {
                // Check if current date is intersecting the first start toll's period in current partition
                List<Toll> currentPartition = intersectingDateCharges.get(partitions-1);
                Toll startToll = currentPartition.get(0);
                DateTime maxTakeChargePeriod = startToll.getTollDateTime().plusMinutes(CHARGE_PERIOD_IN_MINUTES);

                // Within first toll's charge period
                if (maxTakeChargePeriod.isAfter(dateTime)) {
                    currentPartition.add(toll);
                } else {
                    // Outside period -> new partition
                    createAndAddPartition(intersectingDateCharges, toll);
                    partitions++;
                }
            }
        }
        return intersectingDateCharges;
    }

    public TollFee getTollFeeForTime(LocalTime localTime) {
        Map.Entry<LocalTime, TollFee> previousEntry = null;
        for (Map.Entry<LocalTime, TollFee> entry : TOLL_FEE_MAP.entrySet()) {
            if (previousEntry != null && entry.getKey().isAfter(localTime)) {
                return previousEntry.getValue();
            } else if (entry.getKey().equals(localTime)) {
                return entry.getValue();
            }
            previousEntry = entry;
        }
        return previousEntry != null ? previousEntry.getValue() : null;
    }

    private boolean isTollable(Vehicle vehicle, Date date) {
        if (isTollFreeVehicle(vehicle)) {
            return false;
        }
        return !isTollFreeDate(date);
    }

    public boolean isTollFreeVehicle(Vehicle vehicle) {
        return vehicle != null && vehicle.getType().isTollFree();
    }

    public Boolean isTollFreeDate(Date date) {
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);

        int dayOfWeek =  calendar.get(Calendar.DAY_OF_WEEK);
        int month = calendar.get(Calendar.MONTH);
        if (DateHandler.isWeekendDay(dayOfWeek) || month == Calendar.JULY) {
            return true;
        }

        LocalDate localDate = DateHandler.getLocalDateFromDate(date);
        return dateHandler.getHolidayDateSet().contains(localDate);
    }
}