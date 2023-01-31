import com.toll.calculator.DateHandler;
import com.toll.calculator.Toll;
import com.toll.calculator.TollCalculator;
import com.toll.calculator.exception.DateException;
import com.toll.calculator.exception.VehicleException;
import com.toll.calculator.vehicle.Vehicle;
import com.toll.calculator.vehicle.VehicleFactory;
import org.joda.time.DateTime;
import org.junit.jupiter.api.*;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.LocalTime;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

@TestInstance(TestInstance.Lifecycle.PER_CLASS)
public class TestTollCalculator {
    private TollCalculator tollCalculator;
    private TollCalculator apiTollCalculator;
    private SimpleDateFormat sdf;
    private Vehicle privateCar;
    private Vehicle diplomatCar;
    private Vehicle emergencyVehicle;
    private Vehicle bus;
    private Vehicle miniBus;
    private Vehicle motorCycle;
    private Vehicle military;
    private Vehicle tractor;
    private Vehicle truck;
    // Holiday in Sweden
    private Date firstOfMay;
    // July free month
    private Date julyMonth;
    // Usual working day
    private Date workingDay;

    @BeforeAll
    void setUp() throws VehicleException, ParseException {
        // Using predefined holiday dates for year 2023
        tollCalculator = new TollCalculator(new DateHandler());
        // Using dateHandler's API to fetch holiday dates from
        apiTollCalculator = new TollCalculator(new DateHandler(2023));

        sdf = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS");
        privateCar = VehicleFactory.getVehicle(Vehicle.VehicleType.PRIVATE_CAR, null);
        diplomatCar = VehicleFactory.getVehicle(Vehicle.VehicleType.DIPLOMAT_PRIVATE_CAR, null);
        emergencyVehicle = VehicleFactory.getVehicle(Vehicle.VehicleType.EMERGENCY, null);
        bus = VehicleFactory.getVehicle(Vehicle.VehicleType.BUS, null);
        miniBus = VehicleFactory.getVehicle(Vehicle.VehicleType.MINIBUS, null);
        motorCycle = VehicleFactory.getVehicle(Vehicle.VehicleType.MOTORCYCLE, null);
        military = VehicleFactory.getVehicle(Vehicle.VehicleType.MILITARY, null);
        tractor = VehicleFactory.getVehicle(Vehicle.VehicleType.TRACTOR, null);
        truck = VehicleFactory.getVehicle(Vehicle.VehicleType.TRUCK, null);
        firstOfMay = sdf.parse("2023-05-01 14:00:00.123");
        julyMonth = sdf.parse("2023-07-19 14:00:00.123");
        workingDay = sdf.parse("2023-03-15 13:30:30.000");
    }

    @AfterAll
    void tearDown() {
        tollCalculator = null;
        apiTollCalculator = null;
        sdf = null;
        privateCar = null;
        diplomatCar = null;
        emergencyVehicle = null;
        bus = null;
        miniBus = null;
        motorCycle = null;
        military = null;
        tractor = null;
        truck = null;
        firstOfMay = null;
        julyMonth = null;
        workingDay = null;
    }

    @Test
    @DisplayName("Ensure a private car is tollable")
    void testPrivateCarIsTollable() {
        assertFalse(tollCalculator.isTollFreeVehicle(privateCar));
    }

    @Test
    @DisplayName("Ensure a diplomat car is NOT tollable")
    void testDiplomatCarIsNotTollable() {
        assertTrue(tollCalculator.isTollFreeVehicle(diplomatCar));
    }

    @Test
    @DisplayName("Ensure an emergency vehicle is NOT tollable")
    void testEmergencyVehicleIsNotTollable() {
        assertTrue(tollCalculator.isTollFreeVehicle(emergencyVehicle));
    }

    @Test
    @DisplayName("Ensure a bus is NOT tollable")
    void testBusIsNotTollable() {
        assertTrue(tollCalculator.isTollFreeVehicle(bus));
    }

    @Test
    @DisplayName("Ensure a minibus is tollable")
    void testMiniBusIsTollable() {
        assertFalse(tollCalculator.isTollFreeVehicle(miniBus));
    }

    @Test
    @DisplayName("Ensure a motorcycle is NOT tollable")
    void testMotorCycleIsNotTollable() {
        assertTrue(tollCalculator.isTollFreeVehicle(motorCycle));
    }

    @Test
    @DisplayName("Ensure a military vehicle is NOT tollable")
    void testMilitaryVehicleIsNotTollable() {
        assertTrue(tollCalculator.isTollFreeVehicle(military));
    }

    @Test
    @DisplayName("Ensure a tractor is NOT tollable")
    void testTractorIsNotTollable() {
        assertTrue(tollCalculator.isTollFreeVehicle(tractor));
    }

    @Test
    @DisplayName("Ensure a truck is tollable")
    void testTruckIsTollable() {
        assertFalse(tollCalculator.isTollFreeVehicle(truck));
    }

    @Test
    @DisplayName("Ensure first of may is a toll free date")
    void testFirstOfMayIsTollFree() {
        assertTrue(tollCalculator.isTollFreeDate(firstOfMay));
    }

    @Test
    @DisplayName("Ensure that july is a toll free month")
    void testJulyIsTollFree() {
        assertTrue(tollCalculator.isTollFreeDate(julyMonth));
    }

    @Test
    @DisplayName("Ensure working date is not a toll free date")
    void testWorkingDateIsNotTollFree() {
        assertFalse(tollCalculator.isTollFreeDate(workingDay));
    }

    @Test
    @DisplayName("Check timestamp that corresponds to free toll returns free toll")
    void testFreeTollFee() {
        LocalTime freeTime = LocalTime.of(5, 39,0);
        TollCalculator.TollFee tollFee = tollCalculator.getTollFeeForTime(freeTime);
        assertEquals(TollCalculator.TollFee.FREE, tollFee);
    }

    @Test
    @DisplayName("Check timestamp that corresponds to low toll fee returns low toll fee")
    void testLowTollFee() {
        LocalTime lowTime = LocalTime.of(6, 10,0);
        TollCalculator.TollFee tollFee = tollCalculator.getTollFeeForTime(lowTime);
        assertEquals(TollCalculator.TollFee.LOW, tollFee);
    }

    @Test
    @DisplayName("Check timestamp that corresponds to mid toll fee returns mid toll fee")
    void testMidTollFee() {
        LocalTime midTime = LocalTime.of(15, 29,59, 999);
        TollCalculator.TollFee tollFee = tollCalculator.getTollFeeForTime(midTime);
        assertEquals(TollCalculator.TollFee.MID, tollFee);
    }

    @Test
    @DisplayName("Check timestamp that corresponds to high toll fee returns high toll fee")
    void testHighTollFee() {
        LocalTime highTime = LocalTime.of(15, 30,0, 0);
        TollCalculator.TollFee tollFee = tollCalculator.getTollFeeForTime(highTime);
        assertEquals(TollCalculator.TollFee.HIGH, tollFee);
    }

    @Test
    @DisplayName("Ensure partitioning of intersecting dates works")
    void testIntersectingDatesPartitioning() throws ParseException {
        Date date1Partition1 = sdf.parse("2023-01-03 03:00:00.000");
        DateTime dateTime1Partition1 = new DateTime(date1Partition1);
        Date date2Partition1 = sdf.parse("2023-01-03 03:59:59.999");
        DateTime dateTime2Partition1 = new DateTime(date2Partition1);
        Date date1Partition2 = sdf.parse("2023-01-03 04:00:00.000");
        DateTime dateTime1Partition2 = new DateTime(date1Partition2);

        List<Date> dateList = new ArrayList<>();
        dateList.add(date1Partition1);
        dateList.add(date2Partition1);
        dateList.add(date1Partition2);

        // Expected results:
        // Partition 1: date1Partition1 and date2Partition1
        // Partition 2: date1Partition2
        List<List<Toll>> intersectingDateCharges = tollCalculator.getIntersectingDateCharges(privateCar, dateList);
        assertEquals(intersectingDateCharges.size(), 2);
        int partition = 1;
        for (List<Toll> partitionList : intersectingDateCharges) {
            for (Toll toll : partitionList) {
                if (partition == 1) {
                    if (toll.getTollDateTime().equals(dateTime1Partition1) ||
                            toll.getTollDateTime().equals(dateTime2Partition1)) {
                        assertTrue(true);
                    } else {
                        fail();
                    }
                } else {
                    if (toll.getTollDateTime().equals(dateTime1Partition2)) {
                        assertTrue(true);
                    } else {
                        fail();
                    }
                }
            }
            partition++;
        }
    }

    @Test
    @DisplayName("Checks that the calculation of toll fee is correct -> 47")
    void testCalculationOfTheTollFee() throws ParseException, DateException {
        // 0kr
        Date date1Partition1 = sdf.parse("2023-01-03 03:00:00.000");
        // 8 kr
        Date date1Partition2 = sdf.parse("2023-01-03 06:00:00.000");
        // 8 kr
        Date date2Partition2 = sdf.parse("2023-01-03 06:15:00.000");
        // 18 kr
        Date date1Partition3 = sdf.parse("2023-01-03 07:40:00.000");
        // 13 kr
        Date date2Partition3 = sdf.parse("2023-01-03 08:15:00.000");
        // 8 kr
        Date date3Partition3 = sdf.parse("2023-01-03 08:39:59.999");
        // 13 kr
        Date date1Partition4 = sdf.parse("2023-01-03 15:20:00.000");
        // 8 kr
        Date date1Partition5 = sdf.parse("2023-01-03 18:15:00.000");
        // 0
        Date date1Partition6 = sdf.parse("2023-01-03 20:20:00.000");
        // Partition 1: 0 kr
        // Partition 2: 8 kr
        // Partition 3: 18 kr
        // Partition 4: 13 kr
        // Partition 5: 8 kr
        // Partition 6: 0 kr
        // Totally: 47

        // Adding dates in shuffled order here
        Date[] dates = new Date[]{
                date3Partition3, date2Partition3, date1Partition3,
                date1Partition1, date2Partition2, date1Partition2,
                date1Partition5, date1Partition6, date1Partition4
        };

        int totalFee = tollCalculator.getTollFee(privateCar, dates);
        assertEquals(47, totalFee);
    }

    @Test
    @DisplayName("Checks that the calculation of toll fee is returning 0 for toll free vehicles")
    void testCalculationOfTollFreeForTollFreeVehicles() throws ParseException, DateException {
        // 0kr
        Date date1Partition1 = sdf.parse("2023-01-03 03:00:00.000");
        // 8 kr
        Date date1Partition2 = sdf.parse("2023-01-03 06:00:00.000");
        // 8 kr
        Date date2Partition2 = sdf.parse("2023-01-03 06:15:00.000");
        // Partition 1: 0 kr
        // Partition 2: 8 kr

        // Adding dates in shuffled order here
        Date[] dates = new Date[]{
                date1Partition1, date2Partition2, date1Partition2
        };

        int totalFeeMotorCycle = tollCalculator.getTollFee(motorCycle, dates);
        int totalFeeDiplomatCar = tollCalculator.getTollFee(diplomatCar, dates);
        int totalFeeMilitary = tollCalculator.getTollFee(diplomatCar, dates);
        assertEquals(0, totalFeeMotorCycle);
        assertEquals(0, totalFeeDiplomatCar);
        assertEquals(0, totalFeeMilitary);
    }

    @Test
    @DisplayName("Checks that the calculation of toll fee is returning 0 for toll free dates")
    void testCalculationOfTollFreeForTollFreeDate() throws ParseException, DateException {
        // 8kr
        Date date1Partition1 = sdf.parse("2023-01-01 12:00:00.000");
        // 18 kr
        Date date1Partition2 = sdf.parse("2023-01-01 15:30:00.000");
        // 18 kr
        Date date2Partition2 = sdf.parse("2023-01-01 16:00:00.000");
        // Partition 1: 8 kr
        // Partition 2: 18 kr

        // Adding dates in shuffled order here
        Date[] dates = new Date[]{
                date1Partition1, date2Partition2, date1Partition2
        };

        int totalFee = tollCalculator.getTollFee(privateCar, dates);
        assertEquals(0, totalFee);
    }

    @Test
    @DisplayName("Checks that the calculation of toll fee limits fees to not exceed 60")
    void testCalculationOfTheTollFeeToLimitFees() throws ParseException, DateException {
        // 0kr
        Date date1Partition1 = sdf.parse("2023-01-03 03:00:00.000");
        // 8 kr
        Date date1Partition2 = sdf.parse("2023-01-03 06:00:00.000");
        // 8 kr
        Date date2Partition2 = sdf.parse("2023-01-03 06:15:00.000");
        // 18 kr
        Date date1Partition3 = sdf.parse("2023-01-03 07:40:00.000");
        // 13 kr
        Date date2Partition3 = sdf.parse("2023-01-03 08:15:00.000");
        // 8 kr
        Date date3Partition3 = sdf.parse("2023-01-03 08:39:59.999");
        // 13 kr
        Date date1Partition4 = sdf.parse("2023-01-03 15:20:00.000");
        // 18 kr
        Date date2Partition4 = sdf.parse("2023-01-03 15:40:00.000");
        // 13 kr
        Date date1Partition5 = sdf.parse("2023-01-03 17:00:00.000");
        // 8 kr
        Date date1Partition6 = sdf.parse("2023-01-03 18:15:00.000");
        // 0
        Date date1Partition7 = sdf.parse("2023-01-03 20:20:00.000");
        // Partition 1: 0 kr
        // Partition 2: 8 kr
        // Partition 3: 18 kr
        // Partition 4: 18 kr
        // Partition 5: 13 kr
        // Partition 6: 8 kr
        // Partition 7: 0 kr
        // Totally: 65 -> should be limited to 60 by calculator

        // Adding dates in shuffled order here
        Date[] dates = new Date[]{
                date3Partition3, date2Partition3, date1Partition3, date1Partition7,
                date1Partition1, date2Partition2, date1Partition2, date1Partition5,
                date1Partition6, date2Partition4, date1Partition4
        };

        int totalFee = tollCalculator.getTollFee(privateCar, dates);
        assertEquals(60, totalFee);
    }

    @Test
    @DisplayName("Checks that the calculation of toll fee is correct for dynamical date handler-> 52")
    void testCalculationOfTheTollFeeWithDynamicalDateHandler() throws ParseException, DateException {
        // 0kr
        Date date1Partition1 = sdf.parse("2023-01-03 03:00:00.000");
        // 8 kr
        Date date1Partition2 = sdf.parse("2023-01-03 06:00:00.000");
        // 8 kr
        Date date2Partition2 = sdf.parse("2023-01-03 06:15:00.000");
        // 18 kr
        Date date1Partition3 = sdf.parse("2023-01-03 07:40:00.000");
        // 13 kr
        Date date2Partition3 = sdf.parse("2023-01-03 08:15:00.000");
        // 8 kr
        Date date3Partition3 = sdf.parse("2023-01-03 08:39:59.999");
        // 13 kr
        Date date1Partition4 = sdf.parse("2023-01-03 15:20:00.000");
        // 18 kr
        Date date2Partition4 = sdf.parse("2023-01-03 15:40:00.000");
        // 8 kr
        Date date1Partition5 = sdf.parse("2023-01-03 18:15:00.000");
        // 0
        Date date1Partition6 = sdf.parse("2023-01-03 20:20:00.000");
        // Partition 1: 0 kr
        // Partition 2: 8 kr
        // Partition 3: 18 kr
        // Partition 4: 18 kr
        // Partition 5: 8 kr
        // Partition 6: 0 kr
        // Totally: 52

        // Adding dates in shuffled order here
        Date[] dates = new Date[]{
                date3Partition3, date2Partition3, date1Partition3,
                date1Partition1, date2Partition2, date1Partition2, date1Partition5,
                date1Partition6, date2Partition4, date1Partition4
        };

        int totalFee = apiTollCalculator.getTollFee(privateCar, dates);
        assertEquals(52, totalFee);
    }
}
