package com.toll.calculator.service;

import com.toll.calculator.dto.TollRequestWithVehicleType;
import com.toll.calculator.dto.TollResponse;
import com.toll.calculator.dto.vehicle.VehicleTollRequest;
import com.toll.calculator.exception.ResourceNotFoundException;
import com.toll.calculator.exception.TollException;
import com.toll.calculator.model.City;
import com.toll.calculator.model.Holiday;
import com.toll.calculator.model.Rate;
import com.toll.calculator.model.vehicle.Vehicle;
import com.toll.calculator.model.vehicle.VehicleType;
import com.toll.calculator.repository.*;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.time.*;
import java.util.Date;
import java.util.List;
import java.util.Optional;
import java.util.concurrent.TimeUnit;

import static com.toll.calculator.util.DateUtil.getDates;

@Service
@Slf4j
public class TollCalculatorService {

    @Autowired
    private CityRepository cityRepository;

    @Autowired
    private HolidayRepository holidayRepository;

    @Autowired
    private RateRepository rateRepository;

    @Autowired
    private VehicleRepository vehicleRepository;

    @Autowired
    private VehicleTypeRepository vehicleTypeRepository;


    /**
     *
     * @param tollRequest the request that comes from the user.
     *                    the object contains the following fields:
     *                    <li><b>vehicleTypeId:</b> the id of the vehicle type.</li>
     *                    <li><b>vehicleTypeName:</b> the name of the vehicle type.</li>
     *                    <li><b>cityCode:</b> the city code the vehicle has passed toll points.</li>
     *                    <li><b>dates:</b> the dates when vehicle has passed toll point.</li>
     * @return the total fee as TollResponse
     * @throws TollException
     */
    public TollResponse getTollFee(TollRequestWithVehicleType tollRequest) throws TollException {
        VehicleType vehicleType;
        if (tollRequest.getVehicleTypeId() != null) {
            vehicleType = vehicleTypeRepository.findById(tollRequest.getVehicleTypeId()).orElseThrow(()-> new ResourceNotFoundException("Vehicle type not found with id: " + tollRequest.getVehicleTypeId()));
        } else {
            vehicleType = vehicleTypeRepository.findVehicleTypeByName(tollRequest.getVehicleTypeName()).orElseThrow(()-> new ResourceNotFoundException("Vehicle type not found with name: " + tollRequest.getVehicleTypeName()));
        }
        TollResponse tollResponse = new TollResponse();
        Date[] dates = getDates(tollRequest.getTollDates());
        if (vehicleType.isTollFree()){
            tollResponse.setToll(0.0d);
            return tollResponse;
        }else{
            City city = cityRepository.findOneByCode(tollRequest.getCityCode());
            if (city == null) {
                log.warn("Invalid City");
                throw new TollException("Invalid City");
            }
            Date intervalStart = dates[0];
            int totalFee = 0;
            for (Date date : dates) {
                double nextFee = getTollFee(date, city);
                double tempFee = getTollFee(intervalStart, city);

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
            tollResponse.setToll(totalFee);
            return tollResponse;
        }
    }

    public TollResponse getTollFee(VehicleTollRequest tollRequest) throws TollException {
        TollResponse tollResponse = new TollResponse();
        Vehicle vehicle = vehicleRepository.findVehicleByRegNumber(tollRequest.getRegNumber());
        List<Date> dates = vehicle.getTollDates();
        if (vehicle.getVehicleType().isTollFree()){
            tollResponse.setToll(0.0d);
            return tollResponse;
        }else{
            City city = cityRepository.findOneByCode(tollRequest.getCityCode());
            if (city == null) {
                log.warn("Invalid City");
                throw new TollException("Invalid City");
            }
            Date intervalStart = dates.get(0);
            int totalFee = 0;
            for (Date date : dates) {
                double nextFee = getTollFee(date, city);
                double tempFee = getTollFee(intervalStart, city);

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
            tollResponse.setToll(totalFee);
            return tollResponse;
        }
    }


    /**
     *
     * @param localDate the time when vehicle passed the toll point
     * @param rate the rate on that particular point
     * @return the calculated fee on that particular time as Optional
     */
    private Optional<Double> getRateByTime(LocalDateTime localDate, Rate rate) {
        LocalDateTime startDate = convertToLocalDateTimeViaMilisecond(rate.getStartDate());
        LocalDateTime endDate = convertToLocalDateTimeViaMilisecond(rate.getEndDate());

        LocalDateTime startTime = localDate.withHour(startDate.getHour()).withMinute(startDate.getMinute());
        LocalDateTime endTime = localDate.withHour(endDate.getHour()).withMinute(endDate.getMinute());

        if (!localDate.isBefore(startTime) && localDate.isBefore(endTime)) {
            return Optional.of(rate.getTax());
        }

        return Optional.empty();
    }

    public double getTollFee(Date date, City city)
    {
        if (isTollFreeDate(date, city)) {
            return 0.0d;
        }
        LocalDateTime localDate = convertToLocalDateTimeViaMilisecond(date);

        List<Rate> rates = rateRepository.findByCity(city);

        Optional<Double> taxRate = rates.stream()
                .map(rate -> getRateByTime(localDate, rate))
                .filter(Optional::isPresent)
                .map(Optional::get)
                .findFirst();

        return taxRate.orElse(0.0);
    }



    private Boolean isTollFreeDate(Date date, City city)
    {
        LocalDate localDate = convertToLocalDateTimeViaMilisecond(date).toLocalDate();

        if (localDate.getDayOfWeek() == DayOfWeek.SATURDAY || localDate.getDayOfWeek() == DayOfWeek.SUNDAY) {
            return true;
        }

        List<Holiday> holidays = holidayRepository.findByCity(city);

        // Check if the day and month match any holiday (ignoring the year)
        return holidays.stream().anyMatch(holiday -> {
            LocalDate holidayDate = convertToLocalDateTimeViaMilisecond(holiday.getDate()).toLocalDate();
            return localDate.getMonth() == holidayDate.getMonth() && localDate.getDayOfMonth() == holidayDate.getDayOfMonth();
        });
    }


    public LocalDateTime convertToLocalDateTimeViaMilisecond(Date dateToConvert) {
        return Instant.ofEpochMilli(dateToConvert.getTime()).atZone(ZoneId.systemDefault()).toLocalDateTime();
    }
}
