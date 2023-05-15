package com.tollcalculator.service;

import com.tollcalculator.boObject.TollCalculatorRequest;
import com.tollcalculator.boObject.TollCalculatorResponse;
import com.tollcalculator.boObject.VehicleType;
import com.tollcalculator.exceptions.InvalidParameterException;
import com.tollcalculator.model.CityTaxRate;
import com.tollcalculator.model.Vehicle;
import com.tollcalculator.repository.CityRepository;
import com.tollcalculator.repository.vehicleRepository;
import com.tollcalculator.model.City;
import com.tollcalculator.util.DateTimeUtil;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;
import java.time.LocalTime;
import java.time.ZoneId;
import java.util.*;
import java.util.concurrent.TimeUnit;

@Service
public class TollCalculatorServiceImpl implements TollCalculatorService {
    private static final Logger LOG = LoggerFactory.getLogger(TollCalculatorServiceImpl.class);
    private final CityRepository cityRepository;
    private final vehicleRepository vehicleRepository;

    public TollCalculatorServiceImpl(CityRepository cityRepository, vehicleRepository vehicleRepository) {
        this.cityRepository = cityRepository;
        this.vehicleRepository = vehicleRepository;
    }

    @Override
    public void validateRequest(TollCalculatorRequest tollCalculatorRequest) throws InvalidParameterException {
            isValidCity(tollCalculatorRequest.getCity());
            isValidVehicle(tollCalculatorRequest.getVehicle());
        }

    @Override
    public void isValidCity(String city) throws InvalidParameterException {
        if (cityRepository.findByName(city).isEmpty()) {
            throw new InvalidParameterException("Invalid City requested");
        }
    }

    @Override
    public void isValidVehicle(VehicleType vehicle) throws InvalidParameterException {
        if (vehicleRepository.findVehicleByName(vehicle.getType()).isEmpty()) {
            throw new InvalidParameterException("Invalid Vehicle requested");
        }
    }

    /** Calculate Toll Fee
     toll is fee-free for exempted vehicle
     toll is fee-free if weekends and holidays
     toll is charged in the case of multiple fees in the same hour period, the highest one applies.
     toll is differ between 8 SEK and 18 SEK, depending on the time of day
     **/
    @Override
    public TollCalculatorResponse getTaxAmount(String cityName, VehicleType vehicleType, List<Date> dates) {
        City cityObject = cityRepository.findByName(cityName).get();

        // calculate tax for exempt vehicle
        if (IsTollFreeVehicle(cityObject.getTaxExemptVehicles(), vehicleType)) {
            LOG.debug("Requested vehicle {} has free toll", vehicleType.getType());
            return TollCalculatorResponse.builder().taxAmount(new BigDecimal(0)).build();
        }

        // if dates is null or empty
        if (dates == null || dates.isEmpty()) {
            return TollCalculatorResponse.builder().taxAmount(new BigDecimal(0)).build();
        }

        DateTimeUtil.sortDateByAsc(dates);
        Integer singleChargeIntervalInMin = cityObject.getCityTaxChoice().getSingleChargeIntervalInMin();
        Date intervalStart = dates.get(0);
        int totalFee = 0;
        for (Date date : dates) {
            int nextFee = getTollFee(date, cityObject);
            int tempFee = getTollFee(intervalStart, cityObject);

            TimeUnit timeUnit = TimeUnit.MINUTES;
            long diffInMillies = date.getTime() - intervalStart.getTime();
            long minutes = timeUnit.convert(diffInMillies, TimeUnit.MILLISECONDS);

            if (minutes <= singleChargeIntervalInMin) {
                if (totalFee > 0) {
                    totalFee -= tempFee;
                }
                if (nextFee >= tempFee) {
                    tempFee = nextFee;
                }
                totalFee += tempFee;
            } else {
                totalFee += nextFee;
            }
        }
        Integer maxTaxPerDay = cityObject.getCityTaxChoice().getMaxTaxPerDay();
        if (totalFee > maxTaxPerDay) {
            LOG.debug("Eligible for Capping Requested vehicle {} tax fees to max fees per day", vehicleType.getType());
            totalFee = maxTaxPerDay;
        }
        return TollCalculatorResponse.builder().taxAmount(new BigDecimal(totalFee)).build();
    }

    public int getTollFee(final Date date, City city) {
        if (IsTollFreeDate(date, city)) {
            return 0;
        }
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);
        int charge = getTollFeeByTariffAndDate(date, city.getCityTaxRates());
        return charge;
    }

    private int getTollFeeByTariffAndDate(Date date, Set<CityTaxRate> tariffs) {
        BigDecimal totalFee = new BigDecimal(0);
        if (tariffs == null || tariffs.isEmpty()) return totalFee.intValue();

        for (CityTaxRate tariffEntity : tariffs) {
            LocalTime fromTime = tariffEntity.getFromTime();
            LocalTime toTime = tariffEntity.getToTime();
            LocalTime source = date.toInstant().atZone(ZoneId.systemDefault()).toLocalTime();
            if (!source.isBefore(fromTime) && source.isBefore(toTime)) {
                return totalFee.add(tariffEntity.getCharge()).intValue();
            }
        }
        return totalFee.intValue();
    }

    private boolean IsTollFreeVehicle(Set<Vehicle> taxExemptVehicles, VehicleType vehicle) {
        if (taxExemptVehicles == null) return false;
        if (taxExemptVehicles.stream().filter(taxExemptVehicle ->
                taxExemptVehicle.getName().equalsIgnoreCase(vehicle.getType())).count() > 0) return true;
        return false;
    }

    private Boolean IsTollFreeDate(Date date, City city) {
        Calendar calendar = GregorianCalendar.getInstance();
        calendar.setTime(date);
        int month = calendar.get(Calendar.MONTH);
        int dayOfWeek = calendar.get(Calendar.DAY_OF_WEEK);

        if (DateTimeUtil.getWeekendFromCalendar(city.getCityWorkingCalender(), dayOfWeek)) {
            LOG.debug("Requested city {} has toll free weekend", city.getName());
            return true;
        }

        if (DateTimeUtil.getCityHolidayMonthFromCalendar(city.getCityHolidayMonthCalender(), month)) {
            LOG.debug("Requested city {} has toll free holiday month", city.getName());
            return true;
        }

        if (DateTimeUtil.getDateBeforeHolidayORPublicHoliday(date, city)) {
            LOG.debug("Requested city {} has toll free date before holiday", city.getName());
            return true;
        }
        return false;
    }
}
