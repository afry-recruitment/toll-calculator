import com.afry.timesandfees.*;
import com.afry.tollfreedates.*;
import com.afry.vehicles.*;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.time.LocalDate;

public class TollCalculatorTests {

    @Test
    public void testIsVehicleNull() {
        Assertions.assertEquals(new FeeTotal().calculate(
                null, new TestTime().rushHourPriceTime1), -1);
    }

    @Test
    public void testIsDateNull() {
        Assertions.assertEquals(new FeeTotal().calculate(new Car(), null), -1);
    }

    @Test
    public void testAreAllDatesSameDay() {
        Assertions.assertEquals(new FeeTotal().calculate(
                new Car(), new TestTime().rushHourPriceTime1, new TestTime().lowPriceTime,
                new TestTime().nextDayTime), -1);
    }

    @Test
    public void testIsTollFreeVehicle() {
        // Motorbike is free
        Assertions.assertTrue(new Motorbike().isTollFree());
        // Tractor is free
        Assertions.assertTrue(new Tractor().isTollFree());
        // Car is not free
        Assertions.assertFalse(new Car().isTollFree());
    }

    @Test
    public void testIsTollFreeDate() {
        // Weekend
        Assertions.assertTrue(new TollFreeDateCheck().isTollFreeDate(
                LocalDate.of(2022, 11, 12)));
        // Holiday
        Assertions.assertTrue(new TollFreeDateCheck().isTollFreeDate(
                LocalDate.of(2022, 12, 25)));
        // July
        Assertions.assertTrue(new TollFreeDateCheck().isTollFreeDate(
                LocalDate.of(2022, 7, 12)));
        // Normal work day
        Assertions.assertFalse(new TollFreeDateCheck().isTollFreeDate(
                LocalDate.of(2022, 11, 3)));
    }

    @Test
    public void testIsEaster() {
        // Check Easter date three different years
        Assertions.assertEquals(new Holidays().getEaster(
                2022), LocalDate.of(2022, 4, 17));
        Assertions.assertEquals(new Holidays().getEaster(
                2013), LocalDate.of(2013, 3, 31));
        Assertions.assertEquals(new Holidays().getEaster(
                2026), LocalDate.of(2026, 4, 5));
    }

    @Test
    public void testIsMidsummer() {
        Assertions.assertEquals(new Holidays().getMidsummer(
                2022), LocalDate.of(2022, 6, 25));
    }

    @Test
    public void testIsAllSaintsDay() {
        Assertions.assertEquals(new Holidays().getAllSaintsDay(
                2022), LocalDate.of(2022, 11, 5));
    }

    @Test
    public void testSingleFee() {
        // test single fee at all fee levels
        Assertions.assertEquals(new FeeTotal().calculate(
                new Car(), new TestTime().rushHourPriceTime1), 18);
        Assertions.assertEquals(new FeeTotal().calculate(
                new Car(), new TestTime().noFeeTime), 0);
        Assertions.assertEquals(new FeeTotal().calculate(
                new Car(), new TestTime().midPriceTime), 13);
        Assertions.assertEquals(new FeeTotal().calculate(
                new Car(), new TestTime().lowPriceTime), 8);
    }

    @Test
    public void testMultipleFee() {
        // test adding fees, 13 + 18 + 8.
        Assertions.assertEquals(new FeeTotal().calculate(
                new Car(), new TestTime().midPriceTime, new TestTime().lowPriceTime,
                new TestTime().rushHourPriceTime2), 39);
        // Also works with reverse chronology.
        Assertions.assertEquals(new FeeTotal().calculate(
                new Car(), new TestTime().rushHourPriceTime2, new TestTime().lowPriceTime,
                new TestTime().midPriceTime), 39);
    }

    @Test
    public void testSingleFeeInSameHour() {
        // test that only highest fee is charged within same hour
        Assertions.assertEquals(new FeeTotal().calculate(
                new Car(), new TestTime().sameHour1, new TestTime().sameHour2, new TestTime().sameHour3,
                new TestTime().sameHour4, new TestTime().sameHour5), 18);
    }

    @Test
    public void testMaxFee() {
        // test max is not exceeded
        Assertions.assertEquals(new FeeTotal().calculate(
                new Car(), new TestTime().rushHourPriceTime1, new TestTime().midPriceTime,
                new TestTime().lowPriceTime, new TestTime().rushHourPriceTime2,
                new TestTime().midPriceTime2), 60);
    }
}