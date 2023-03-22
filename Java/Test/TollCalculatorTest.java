import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

import java.time.LocalDateTime;
import org.junit.jupiter.api.Test;

public class TollCalculatorTest {
    @Test
    public void testGetFeeOnePass() {
        TollCalculator tollCalculator = new TollCalculator();
        Vehicle vehicle = new Car();
        LocalDateTime date = LocalDateTime.of(2023, 3, 22, 7, 30);
        int expectedFee = 18;
        int actualFee = tollCalculator.getFeeOnePass(date, vehicle);
        assertEquals(expectedFee, actualFee);
    }

    @Test
    public void testIsTollFreeDate() {
        TollCalculator tollCalculator = new TollCalculator();
        LocalDateTime tollFreeDate = LocalDateTime.of(2023, 4, 30, 9, 0);
        LocalDateTime notTollFreeDate = LocalDateTime.of(2023, 3, 22, 8, 0);
        Car car = new Car();
        assertEquals(0, tollCalculator.getTollFee(car, tollFreeDate));
        assertEquals(13, tollCalculator.getTollFee(car, notTollFreeDate));
    }

    @Test
    public void testIsTollFreeVehicle() {
        TollCalculator tollCalculator = new TollCalculator();
        LocalDateTime notTollFreeDate = LocalDateTime.of(2023, 3, 22, 8, 0);
        Vehicle diplomat = new Diplomat();
        Vehicle emergency = new Emergency();
        Vehicle foreign = new Foreign();
        Vehicle military = new Military();
        Vehicle motorbike = new Motorbike();
        Vehicle tractor = new Tractor();

        assertEquals(0, tollCalculator.getTollFee(diplomat, notTollFreeDate));
        assertEquals(0, tollCalculator.getTollFee(emergency, notTollFreeDate));
        assertEquals(0, tollCalculator.getTollFee(foreign, notTollFreeDate));
        assertEquals(0, tollCalculator.getTollFee(military, notTollFreeDate));
        assertEquals(0, tollCalculator.getTollFee(motorbike, notTollFreeDate));
        assertEquals(0, tollCalculator.getTollFee(tractor, notTollFreeDate));
    }

    @Test
    public void testGetTollFee() {
        TollCalculator tollCalculator = new TollCalculator();
        Vehicle vehicle = new Car();
        LocalDateTime[] passes = new LocalDateTime[3];
        passes[0] = LocalDateTime.of(2023, 3, 22, 7, 0);
        passes[1] = LocalDateTime.of(2023, 3, 22, 8, 0);
        passes[2] = LocalDateTime.of(2023, 3, 22, 16, 0);
        int expectedFee = 36;
        int actualFee = tollCalculator.getTollFee(vehicle, passes);
        assertEquals(expectedFee, actualFee);
    }

    @Test
    public void testManyTollFeesAnHour() {
        TollCalculator tollCalculator = new TollCalculator();
        Vehicle vehicle = new Car();
        LocalDateTime[] passes = new LocalDateTime[7];
        passes[0] = LocalDateTime.of(2023, 3, 22, 7, 1);
        passes[1] = LocalDateTime.of(2023, 3, 22, 7, 20);
        passes[2] = LocalDateTime.of(2023, 3, 22, 7, 30);
        passes[3] = LocalDateTime.of(2023, 3, 22, 7, 40);
        passes[4] = LocalDateTime.of(2023, 3, 22, 7, 54);
        passes[5] = LocalDateTime.of(2023, 3, 22, 7, 56);
        passes[6] = LocalDateTime.of(2023, 3, 22, 7, 59);
        int expectedFee = 18;
        int actualFee = tollCalculator.getTollFee(vehicle, passes);
        assertEquals(expectedFee, actualFee);
    }

    @Test
    public void testManyTollFeesAnHourPlusExtra() {
        TollCalculator tollCalculator = new TollCalculator();
        Vehicle vehicle = new Car();
        LocalDateTime[] passes = new LocalDateTime[8];
        passes[0] = LocalDateTime.of(2023, 3, 22, 7, 1);
        passes[1] = LocalDateTime.of(2023, 3, 22, 7, 20);
        passes[2] = LocalDateTime.of(2023, 3, 22, 7, 30);
        passes[3] = LocalDateTime.of(2023, 3, 22, 7, 40);
        passes[4] = LocalDateTime.of(2023, 3, 22, 7, 54);
        passes[5] = LocalDateTime.of(2023, 3, 22, 7, 56);
        passes[6] = LocalDateTime.of(2023, 3, 22, 7, 59);
        passes[7] = LocalDateTime.of(2023, 3, 22, 8, 56);
        int expectedFee = 26;
        int actualFee = tollCalculator.getTollFee(vehicle, passes);
        assertEquals(expectedFee, actualFee);
    }

    @Test
    public void testFullTollFee() {
        TollCalculator tollCalculator = new TollCalculator();
        Vehicle vehicle = new Car();
        LocalDateTime[] passes = new LocalDateTime[11];
        passes[0] = LocalDateTime.of(2023, 3, 22, 6, 59);
        passes[1] = LocalDateTime.of(2023, 3, 22, 8, 0);
        passes[2] = LocalDateTime.of(2023, 3, 22, 9, 1);
        passes[3] = LocalDateTime.of(2023, 3, 22, 10, 2);
        passes[4] = LocalDateTime.of(2023, 3, 22, 11, 3);
        passes[5] = LocalDateTime.of(2023, 3, 22, 12, 4);
        passes[6] = LocalDateTime.of(2023, 3, 22, 13, 5);
        passes[7] = LocalDateTime.of(2023, 3, 22, 14, 6);
        passes[8] = LocalDateTime.of(2023, 3, 22, 15, 7);
        passes[9] = LocalDateTime.of(2023, 3, 22, 16, 8);
        passes[10] = LocalDateTime.of(2023, 3, 22, 17, 9);
        int expectedFee = 60;
        int actualFee = tollCalculator.getTollFee(vehicle, passes);
        assertEquals(expectedFee, actualFee);
    }

    @Test
    public void testNoTollDate() {
        TollCalculator tollCalculator = new TollCalculator();
        Vehicle vehicle = new Car();
        LocalDateTime[] passes = new LocalDateTime[11];

        assertThrows(RuntimeException.class, () -> tollCalculator.getTollFee(vehicle, passes));

        assertThrows(RuntimeException.class, () -> tollCalculator.getTollFee(vehicle, null));

        assertThrows(RuntimeException.class, () -> tollCalculator.getTollFee(vehicle));
    }

    @Test
    public void testHolidayDriver() {
        LocalDateTime[] passes = new LocalDateTime[28];
        passes[0] = LocalDateTime.of(2023, 1, 1, 6, 59);
        passes[1] = LocalDateTime.of(2023, 1, 1, 6, 59);
        passes[2] = LocalDateTime.of(2023, 4, 30, 8, 0);
        passes[3] = LocalDateTime.of(2023, 5, 1, 9, 1);
        passes[4] = LocalDateTime.of(2023, 6, 5, 10, 2);
        passes[5] = LocalDateTime.of(2023, 6, 6, 11, 3);
        passes[6] = LocalDateTime.of(2023, 12, 24, 12, 4);
        passes[7] = LocalDateTime.of(2023, 12, 25, 13, 5);
        passes[8] = LocalDateTime.of(2023, 12, 26, 14, 6);
        passes[9] = LocalDateTime.of(2023, 12, 31, 15, 7);

        passes[10] = LocalDateTime.of(2023, 4, 6, 16, 8);
        passes[11] = LocalDateTime.of(2023, 4, 7, 17, 9);
        passes[12] = LocalDateTime.of(2023, 4, 10, 17, 9);
        passes[13] = LocalDateTime.of(2023, 5, 18, 17, 9);
        passes[14] = LocalDateTime.of(2023, 6, 23, 17, 9);
        passes[15] = LocalDateTime.of(2023, 11, 3, 17, 9);

        passes[16] = LocalDateTime.of(2024, 3, 28, 16, 8);
        passes[17] = LocalDateTime.of(2024, 3, 29, 17, 9);
        passes[18] = LocalDateTime.of(2024, 4, 1, 17, 9);
        passes[19] = LocalDateTime.of(2024, 5, 9, 17, 9);
        passes[20] = LocalDateTime.of(2024, 6, 21, 17, 9);
        passes[21] = LocalDateTime.of(2024, 11, 1, 17, 9);

        passes[22] = LocalDateTime.of(2025, 4, 17, 16, 8);
        passes[23] = LocalDateTime.of(2025, 4, 18, 17, 9);
        passes[24] = LocalDateTime.of(2025, 4, 21, 17, 9);
        passes[25] = LocalDateTime.of(2025, 5, 29, 17, 9);
        passes[26] = LocalDateTime.of(2025, 6, 20, 17, 9);
        passes[27] = LocalDateTime.of(2025, 10, 31, 17, 9);

        for (LocalDateTime dateTime : passes) {
            testOneDate(dateTime);
        }
    }
    private void testOneDate(LocalDateTime date) {
        System.out.println(date);
        TollCalculator tollCalculator = new TollCalculator();
        Vehicle vehicle = new Car();

        int actualFee = tollCalculator.getTollFee(vehicle, date);
        assertEquals(0, actualFee);
    }
}
