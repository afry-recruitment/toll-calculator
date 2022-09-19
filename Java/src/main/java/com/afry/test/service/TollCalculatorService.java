package com.afry.test.service;

import java.util.*;
import java.util.concurrent.*;
import java.util.stream.Collectors;

import com.afry.test.dao.TollCalculatorRepository;
import com.afry.test.model.RushHour;
import com.afry.test.model.TollFreeVehicles;
import com.afry.test.model.TollEntry;
import com.afry.test.model.Vehicle;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

/**
 * TollCalculatorService - provide data calculations for toll gate operations
 */
@Service
public class TollCalculatorService {
    @Autowired
    TollCalculatorRepository tollCalculatorRepository;
    @Autowired
    RushHourService rushHourService;

    public TollCalculatorService() {

    }
    /**
     * Calculate the total toll fee for one day
     * @param vehicle - the vehicle
     * @return - the total toll fee for that day
     */
    public int getTollFee(Vehicle vehicle) {
        int totalFee = 0;
        //get all TollEntries for Vehicle
        List<TollEntry> tollEntryList = vehicle.getTollEntries();
        Date intervalStart = tollEntryList.get(0).getEntryDate();

        // Day End time for today
        Calendar calendar = GregorianCalendar.getInstance();
        Date dateNow = new Date();
        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 23);
        calendar.set(Calendar.MINUTE, 59);

        // Day start time for today
        Date dateTo = calendar.getTime();
        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 00);
        calendar.set(Calendar.MINUTE, 00);

        Date dateFrom = calendar.getTime();

        //Filter all TollEntries for match with entry date
        List<TollEntry> tollEntriesFilter = tollEntryList.stream().filter(x -> x.getEntryDate().after(dateFrom) && x.getEntryDate().before(dateTo)).collect(Collectors.toList());

        for (TollEntry tollEntry : tollEntriesFilter) {
            Date date = tollEntry.getEntryDate();
            int nextFee = getTollFee(date, vehicle);
            int tempFee = getTollFee(intervalStart, vehicle);

            TimeUnit timeUnit = TimeUnit.MINUTES;
            long diffInMillies = date.getTime() - intervalStart.getTime();
            long minutes = timeUnit.convert(diffInMillies, TimeUnit.MILLISECONDS);

            if (minutes <= 60) {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            } else {
                totalFee += nextFee;
            }
        }

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    /**
     * Validate Vehicle is Toll Free Vehicle
     * @param vehicle - the vehicle
     * @return - is Toll Free Vehicle
     */
    public boolean isTollFreeVehicle(Vehicle vehicle) {
        if (vehicle == null) return false;
        String vehicleType = vehicle.getType();
        return vehicleType.equals(TollFreeVehicles.MOTORBIKE.getType()) ||
                vehicleType.equals(TollFreeVehicles.TRACTOR.getType()) ||
                vehicleType.equals(TollFreeVehicles.EMERGENCY.getType()) ||
                vehicleType.equals(TollFreeVehicles.DIPLOMAT.getType()) ||
                vehicleType.equals(TollFreeVehicles.FOREIGN.getType()) ||
                vehicleType.equals(TollFreeVehicles.MILITARY.getType());
    }

    /**
     * Calculate Toll Free based on time
     * @param date - Entry date
     * @param vehicle - The vehicle
     * @return - TollFee
     */
    public int getTollFee(final Date date, Vehicle vehicle) {
        if (isTollFreeDate(date) || isTollFreeVehicle(vehicle)) {
            return 0;
        }
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);
        int hour = calendar.get(Calendar.HOUR_OF_DAY);
        int minute = calendar.get(Calendar.MINUTE);
        if (isRushHour(hour)) {
            return 18;
        }
        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else return 0;
    }

    /**
     * Validate Vehicle Entry time is on Rush Hour
     * @param hour
     * @return -Hour Is in between Rush Hour or not
     */
    public Boolean isRushHour(int hour) {
        List<RushHour> rushHoursList = rushHourService.getRushHours();
        for (RushHour rushHour : rushHoursList) {
            if (rushHour.getFromHour() >= hour && rushHour.getToHour() >= hour) {
                return true;
            }
        }
        return false;
    }

    /**
     * Validate Entry for Toll Free Date
     * @param date
     * @return Is Toll Free Date
     */
    public Boolean isTollFreeDate(Date date) {
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DAY_OF_MONTH);

        int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);
        if (dayOfWeek == Calendar.SATURDAY || dayOfWeek == Calendar.SUNDAY) {
            return true;
        }

        if (year == 2022) {
            if (month == Calendar.JANUARY && day == 1 ||
                    month == Calendar.MARCH && (day == 28 || day == 29) ||
                    month == Calendar.APRIL && (day == 1 || day == 30) ||
                    month == Calendar.MAY && (day == 1 || day == 8 || day == 9) ||
                    month == Calendar.JUNE && (day == 5 || day == 6 || day == 21) ||
                    month == Calendar.JULY ||
                    month == Calendar.NOVEMBER && day == 1 ||
                    month == Calendar.DECEMBER && (day == 24 || day == 25 || day == 26 || day == 31)) {
                return true;
            }
        }
        return false;
    }

    /**
     *
     * @param vehicle
     * @return Calculated Toll Fee
     * @throws Exception
     */
    public Integer calculateTollFee(Vehicle vehicle) throws Exception {
        int totalFee = 0;
        Date dateNow = new Date();
        TollEntry tollEntry = new TollEntry();
        tollEntry.setEntryDate(dateNow);
        tollEntry.setVehicle(vehicle);

        try {
            // Check Vehicle with DB is exists or not , if exists add this entry to TollEntries and save.
            Optional<Vehicle> vehicleRe = getVehicle(vehicle.getVehicleId());
            if (vehicleRe.isPresent()) {
                vehicle = vehicleRe.get();
                vehicle.getTollEntries().add(tollEntry);
            } else {
                // if not create new TollEntry for Vehicle and save new Vehicle with tollEntries.
                List<TollEntry> tollEntries = new ArrayList<>();
                tollEntries.add(tollEntry);
                vehicle.setTollEntries(tollEntries);
            }
            totalFee = getTollFee(vehicle);
            tollCalculatorRepository.save(vehicle);

        } catch (Exception e) {
            throw new Exception(e);
        }
        return totalFee;
    }

    /**
      * @param vehicleId
     * @return Vehicle from toll Repository match with given ID
     * @throws Exception
     */
    public Optional<Vehicle> getVehicle(String vehicleId) throws Exception {
        return tollCalculatorRepository.findById(vehicleId);
    }
}

