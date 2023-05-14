package com.tollcalculator.service;

import com.tollcalculator.boObject.TollCalculatorRequest;
import com.tollcalculator.boObject.TollCalculatorResponse;
import com.tollcalculator.boObject.VehicleType;
import com.tollcalculator.repository.CityRepository;
import com.tollcalculator.model.*;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.jupiter.MockitoExtension;

import java.math.BigDecimal;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.LocalTime;
import java.util.*;

import static org.assertj.core.api.Assertions.assertThat;

@ExtendWith(MockitoExtension.class)
public class TollCalculatorServiceImplTest {

    @Mock
    CityRepository cityRepository;

    @InjectMocks
    TollCalculatorServiceImpl congestionTaxCalculatorService;

    @Test
    public void validInputTaxEligible() throws ParseException {
        TollCalculatorRequest request = constructRequest("Car");
        Mockito.when(cityRepository.findByName("Gothenburg")).thenReturn(validVehicleDetails());
        TollCalculatorResponse result = congestionTaxCalculatorService.getTaxAmount("Gothenburg", request.getVehicle(), request.getCheckInTime());
        assertThat(result).isNotNull();
        assertThat(result.getTaxAmount()).isEqualTo(new BigDecimal(8));
    }

    private TollCalculatorRequest constructRequest(String vehicleType) throws ParseException {
        VehicleType vehicle = new VehicleType();
        vehicle.setType(vehicleType);
        List<Date> dateList = new ArrayList<>();
        DateFormat formatter = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        Date dateTime = formatter.parse("2013-01-14 06:00:00");
        dateList.add(dateTime);
        TollCalculatorRequest request = new TollCalculatorRequest();
        request.setCheckInTime(dateList);
        request.setVehicle(vehicle);
        return request;
    }

    private Optional<City> validVehicleDetails() throws ParseException {
        City cityEntity = City.builder().name("Gothenburg").id(1L).build();

        CityWorkingCalendar workingCalendarEntity = CityWorkingCalendar.builder()
                .isSunday(false).isSaturday(false)
                .isMonday(true).isTuesday(true).isWednesday(true).isThursday(true).isFriday(true).build();
        cityEntity.setCityWorkingCalender(workingCalendarEntity);

        CityHolidayMonthCalendar holidayMonthsEntity = CityHolidayMonthCalendar.builder()
                .isJanuary(false).isFebruary(false).isMarch(false).isApril(false).isMay(false).isJune(false)
                .isJuly(true).isAugust(false).isSeptember(false).isOctober(false).isNovember(false).isDecember(false).build();
        cityEntity.setCityHolidayMonthCalender(holidayMonthsEntity);

        CityHolidayCalendar cityHolidayCalendar = CityHolidayCalendar.builder().date(holidayDate()).build();
        cityEntity.setCityHolidayCalenders(new HashSet<>(Arrays.asList(cityHolidayCalendar)));

        Set<CityTaxRate> tariffEntities = new HashSet<>();
        CityTaxRate tariffEntity1 = CityTaxRate.builder()
                .city(cityEntity)
                .fromTime(LocalTime.parse("06:00"))
                .toTime(LocalTime.parse("06:29:59"))
                .charge(BigDecimal.valueOf(8))
                .build();
        tariffEntities.add(tariffEntity1);

        cityEntity.setCityTaxRates(tariffEntities);

        CityTaxChoice cityPreferenceEntity = CityTaxChoice.builder()
                .maxTaxPerDay(60)
                .numberOfTaxFreeDaysBeforeHoliday(1)
                .singleChargeIntervalInMin(60)
                .build();
        cityEntity.setCityTaxChoice(cityPreferenceEntity);

        return Optional.of(cityEntity);
    }

    private Date holidayDate() throws ParseException {
        DateFormat formatter = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
        Date dateTime = formatter.parse("2013-01-18 06:00:00");
        return dateTime;
    }

}
