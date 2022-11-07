import com.afry.tollfreedates.*;
import com.afry.vehicles.*;
import com.afry.*;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import java.time.LocalDate;
import java.util.Calendar;
import java.util.Date;

public class TollCalculatorTests {

    @Test
    public void testIsVehicleNull() {
        Assertions.assertEquals(TollCalculator.getTotalFee(
                null, new Date(122, Calendar.NOVEMBER, 7, 16, 33)), -1);
    }

    @Test
    public void testIsDateNull() {
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), null), -1);
    }

    @Test
    public void testAreAllDatesSameDay() {
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, Calendar.NOVEMBER, 7, 8, 33),
                new Date(122, Calendar.NOVEMBER, 7, 16, 15),
                new Date(122, Calendar.NOVEMBER, 8, 15, 38)), -1);
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
                2022), LocalDate.of(2022,4,17));
        Assertions.assertEquals(new Holidays().getEaster(
                2013), LocalDate.of(2013,3,31));
        Assertions.assertEquals(new Holidays().getEaster(
                2026), LocalDate.of(2026,4,5));
    }

    @Test
    public void testIsMidsummer() {
        Assertions.assertEquals(new Holidays().getMidsummer(
                2022), LocalDate.of(2022,6,25));
    }

    @Test
    public void testIsAllSaintsDay() {
        Assertions.assertEquals(new Holidays().getAllSaintsDay(
                2022), LocalDate.of(2022,11,5));
    }

    /*
    Code example if I wish to avoid Date instances in tests below;
    java.util.Date.from(LocalDateTime.of(2022,11,7,7,27).atZone(ZoneId.systemDefault()).toInstant())
     */
    @Test
    public void testSingleFee() {
        // test single fee at all fee levels
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, Calendar.NOVEMBER, 7, 16, 33)), 18);
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, Calendar.NOVEMBER, 7, 18, 33)), 0);
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, Calendar.OCTOBER, 18, 8, 27)), 13);
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, Calendar.NOVEMBER, 3, 6, 14)), 8);
    }

    @Test
    public void testMultipleFee() {
        // test adding fees, 13 + 18 + 8.
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, Calendar.NOVEMBER, 7, 8, 27),
                new Date(122, Calendar.NOVEMBER, 7, 16, 33),
                new Date(122, Calendar.NOVEMBER, 7, 18, 14)), 39);
    }

    @Test
    public void testSingleFeeInSameHour() {
        // test that only highest fee is charged within same hour
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, Calendar.NOVEMBER, 7, 6, 40),
                new Date(122, Calendar.NOVEMBER, 7, 7, 22)), 18);
    }

    @Test
    public void testMaxFee() {
        // test max is not exceeded
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, Calendar.NOVEMBER, 7, 7, 27),
                new Date(122, Calendar.NOVEMBER, 7, 8, 33),
                new Date(122, Calendar.NOVEMBER, 7, 15, 38),
                new Date(122, Calendar.NOVEMBER, 7, 16, 57),
                new Date(122, Calendar.NOVEMBER, 7, 18, 29)), 60);
    }
}