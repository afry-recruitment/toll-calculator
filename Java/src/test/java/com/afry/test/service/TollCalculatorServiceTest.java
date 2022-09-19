package com.afry.test.service;

import java.text.SimpleDateFormat;
import java.util.*;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mockito;
import org.mockito.MockitoAnnotations;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.test.util.ReflectionTestUtils;

import com.afry.test.dao.RushHourRepository;
import com.afry.test.dao.TollCalculatorRepository;
import com.afry.test.model.*;

/**
 * TollServiceTest to cover and test TollService methods
 */
@ExtendWith(MockitoExtension.class)
public class TollCalculatorServiceTest {
    TollCalculatorService tollCalculatorService;
    @MockBean
    RushHourService rushHourService;
    @MockBean
    RushHourRepository rushHourRepository;
    SimpleDateFormat formatter;
    List<RushHour> rushHoursList;

    @BeforeEach
    public void init() {
        MockitoAnnotations.openMocks(this);
        tollCalculatorService = new TollCalculatorService();
        formatter = new SimpleDateFormat("dd-MM-yyyy HH:mm:ss");
        rushHoursList = new ArrayList();

        RushHour rushHour1 = new RushHour();
        rushHour1.setFromHour(12);
        rushHour1.setToHour(13);
        RushHour rushHour2 = new RushHour();
        rushHour2.setFromHour(16);
        rushHour2.setToHour(18);

        rushHoursList.add(rushHour1);
        rushHoursList.add(rushHour2);

        rushHourService = Mockito.mock(RushHourService.class);
        rushHourRepository = Mockito.mock(RushHourRepository.class);
        TollCalculatorRepository tollCalculatorRepository = Mockito.mock(TollCalculatorRepository.class);
        ReflectionTestUtils.setField(tollCalculatorService, "tollCalculatorRepository", tollCalculatorRepository);
        ReflectionTestUtils.setField(tollCalculatorService, "rushHourService", rushHourService);

    }

    @Test
    @DisplayName("Test Get Toll Fee For Multiple Entries Per Hour")
    void testMaxTollFeeForMultipleEntriesPerHour() throws Exception {
        Calendar calendar = GregorianCalendar.getInstance();
        Date dateNow = new Date();
        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 06);
        calendar.set(Calendar.MINUTE, 20);

        Date date1 = calendar.getTime();
        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 06);
        calendar.set(Calendar.MINUTE, 40);

        Date date2 = calendar.getTime();
        List<TollEntry> tollEntries = new ArrayList<>();
        Vehicle vehicle = new Car();
        TollEntry tollEntry1 = new TollEntry();
        tollEntry1.setEntryDate(date1);
        tollEntry1.setVehicle(vehicle);
        tollEntry1.setId(1);

        TollEntry tollEntry2 = new TollEntry();
        tollEntry2.setEntryDate(date2);
        tollEntry2.setVehicle(vehicle);
        tollEntry2.setId(2);

        tollEntries.add(tollEntry1);
        tollEntries.add(tollEntry2);

        vehicle.setTollEntries(tollEntries);
        Integer actualTollFee = tollCalculatorService.getTollFee(vehicle);

        Assertions.assertNotNull(actualTollFee);
        Assertions.assertEquals(13, actualTollFee);
    }

    @Test
    @DisplayName("Test Get Toll Fee")
    void testGetTollFee() throws Exception {
        List<TollEntry> tollEntries = new ArrayList<>();

        Calendar calendar = GregorianCalendar.getInstance();
        Date dateNow = new Date();
        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 06);
        calendar.set(Calendar.MINUTE, 20);

        Date date1 = calendar.getTime();
        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 07);
        calendar.set(Calendar.MINUTE, 40);
        Date date2 = calendar.getTime();


        Vehicle vehicle = new Car();
        TollEntry tollEntry1 = new TollEntry();
        tollEntry1.setEntryDate(date1);
        tollEntry1.setVehicle(vehicle);
        tollEntry1.setId(1);

        TollEntry tollEntry2 = new TollEntry();
        tollEntry2.setEntryDate(date2);
        tollEntry2.setVehicle(vehicle);
        tollEntry2.setId(2);

        tollEntries.add(tollEntry1);
        tollEntries.add(tollEntry2);

        vehicle.setTollEntries(tollEntries);
        Integer actualTollFee = tollCalculatorService.getTollFee(vehicle);

        Assertions.assertNotNull(actualTollFee);
        Assertions.assertEquals(26, actualTollFee);
    }

    @Test
    @DisplayName("Test Get Toll Fee For Rush Hours")
    void testGetTollFeeForRushHours() throws Exception {
        List<TollEntry> tollEntries = new ArrayList<>();

        Calendar calendar = GregorianCalendar.getInstance();
        Date dateNow = new Date();
        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 11);
        calendar.set(Calendar.MINUTE, 20);

        Date date1 = calendar.getTime();

        Vehicle vehicle = new Car();
        TollEntry tollEntry1 = new TollEntry();
        tollEntry1.setEntryDate(date1);
        tollEntry1.setVehicle(vehicle);
        tollEntry1.setId(1);

        tollEntries.add(tollEntry1);
        vehicle.setTollEntries(tollEntries);

        Mockito.when(rushHourService.getRushHours()).thenReturn(rushHoursList);
        Integer actualTollFee = tollCalculatorService.getTollFee(vehicle);

        Assertions.assertNotNull(actualTollFee);
        Assertions.assertEquals(18, actualTollFee);
    }

    @Test
    @DisplayName("Test Get Toll Free Days")
    void testGetTollFeeForTollFreeDays() throws Exception {
        List<TollEntry> tollEntries = new ArrayList<>();
        String date_string1 = "01-01-2022 11:20:37";
        Date date1 = formatter.parse(date_string1);

        Vehicle vehicle = new Car();
        TollEntry tollEntry1 = new TollEntry();
        tollEntry1.setEntryDate(date1);
        tollEntry1.setVehicle(vehicle);
        tollEntry1.setId(1);

        tollEntries.add(tollEntry1);

        vehicle.setTollEntries(tollEntries);
        Integer actualTollFee = tollCalculatorService.getTollFee(vehicle);

        Assertions.assertNotNull(actualTollFee);
        Assertions.assertEquals(0, actualTollFee);
    }

    @Test
    @DisplayName("Test Get Toll Free Vehicle")
    void testGetTollFeeForTollFreeVehicle() throws Exception {
        List<TollEntry> tollEntries = new ArrayList<>();
        String date_string1 = "01-02-2022 11:20:37";
        Date date1 = formatter.parse(date_string1);

        Vehicle vehicle = new Motorbike();
        TollEntry tollEntry1 = new TollEntry();
        tollEntry1.setEntryDate(date1);
        tollEntry1.setVehicle(vehicle);
        tollEntry1.setId(1);

        tollEntries.add(tollEntry1);

        vehicle.setTollEntries(tollEntries);
        Integer actualTollFee = tollCalculatorService.getTollFee(vehicle);

        Assertions.assertNotNull(actualTollFee);
        Assertions.assertEquals(0, actualTollFee);
    }

    @Test
    @DisplayName("Test Get Toll Free For WeekEnds")
    void testGetTollFeeForTollFreeForWeekEnds() throws Exception {
        List<TollEntry> tollEntries = new ArrayList<>();
        String date_string1 = "15-09-2022 11:20:37";
        Date date1 = formatter.parse(date_string1);

        Vehicle vehicle = new Motorbike();
        TollEntry tollEntry1 = new TollEntry();
        tollEntry1.setEntryDate(date1);
        tollEntry1.setVehicle(vehicle);
        tollEntry1.setId(1);

        tollEntries.add(tollEntry1);

        vehicle.setTollEntries(tollEntries);
        Integer actualTollFee = tollCalculatorService.getTollFee(vehicle);

        Assertions.assertNotNull(actualTollFee);
        Assertions.assertEquals(0, actualTollFee);
    }

    @Test
    @DisplayName("Test Total Toll Fee Per day")
    void testGetTotalTollFeePerday() throws Exception {
        List<TollEntry> tollEntries = new ArrayList<>();
        Calendar calendar = GregorianCalendar.getInstance();
        Date dateNow = new Date();
        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 06);
        calendar.set(Calendar.MINUTE, 20);

        Date date1 = calendar.getTime();
        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 07);
        calendar.set(Calendar.MINUTE, 40);
        Date date2 = calendar.getTime();

        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 10);
        calendar.set(Calendar.MINUTE, 40);
        Date date3 = calendar.getTime();

        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 15);
        calendar.set(Calendar.MINUTE, 40);
        Date date4 = calendar.getTime();

        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 16);
        calendar.set(Calendar.MINUTE, 40);
        Date date5 = calendar.getTime();

        calendar.setTime(dateNow);
        calendar.set(Calendar.HOUR_OF_DAY, 18);
        calendar.set(Calendar.MINUTE, 40);
        Date date6 = calendar.getTime();

        Vehicle vehicle = new Car();
        TollEntry tollEntry1 = new TollEntry();
        tollEntry1.setEntryDate(date1);
        tollEntry1.setVehicle(vehicle);


        TollEntry tollEntry2 = new TollEntry();
        tollEntry2.setEntryDate(date2);
        tollEntry2.setVehicle(vehicle);


        TollEntry tollEntry3 = new TollEntry();
        tollEntry3.setEntryDate(date3);
        tollEntry3.setVehicle(vehicle);


        TollEntry tollEntry4 = new TollEntry();
        tollEntry4.setEntryDate(date4);
        tollEntry4.setVehicle(vehicle);

        TollEntry tollEntry5 = new TollEntry();
        tollEntry5.setEntryDate(date5);
        tollEntry5.setVehicle(vehicle);


        TollEntry tollEntry6 = new TollEntry();
        tollEntry6.setEntryDate(date6);
        tollEntry6.setVehicle(vehicle);


        tollEntries.add(tollEntry1);
        tollEntries.add(tollEntry2);
        tollEntries.add(tollEntry3);
        tollEntries.add(tollEntry4);
        tollEntries.add(tollEntry5);
        tollEntries.add(tollEntry6);

        vehicle.setTollEntries(tollEntries);
        Integer actualTollFee = tollCalculatorService.getTollFee(vehicle);

        Assertions.assertNotNull(actualTollFee);
        Assertions.assertEquals(60, actualTollFee);
    }
}
