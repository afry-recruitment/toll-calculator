import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.time.LocalDate;
import java.util.Date;

public class Tests {

    @Test
    public void testIsTollFreeVehicle() {
        // Motorbike is free
        Assertions.assertEquals(new Motorbike().isTollFree(), true);
        // Tractor is free
        Assertions.assertEquals(new Tractor().isTollFree(), true);
        // Car is not free
        Assertions.assertEquals(new Car().isTollFree(), false);
    }

    @Test
    public void testIsTollFreeDate() {
        // Weekend
        Assertions.assertEquals(new TollFreeDateCheck().isTollFreeDate(
                LocalDate.of(2022,11,12)), true);
        // Holiday
        Assertions.assertEquals(new TollFreeDateCheck().isTollFreeDate(
                LocalDate.of(2022,12,25)), true);
        // July
        Assertions.assertEquals(new TollFreeDateCheck().isTollFreeDate(
                LocalDate.of(2022,07,12)), true);
        // Normal work day
        Assertions.assertEquals(new TollFreeDateCheck().isTollFreeDate(
                LocalDate.of(2022,11,3)), false);
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

    @Test
    public void testSingleFee() {
        // test all fee levels
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, 10, 7, 16, 33)), 18);
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, 10, 7, 18, 33)), 0);
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, 9, 18, 8, 27)), 13);
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, 10, 3, 6, 14)), 8);
    }

    @Test
    public void testMultipleFee() {
        // test adding fees, 13 + 18 + 8.
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, 10, 7, 8, 27),
                new Date(122, 10, 7, 16, 33),
                new Date(122, 10, 7, 18, 14)), 39);
    }

    @Test
    public void testSingleFeeInSameHour() {
        // test only highest fee is charged within same hour
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, 10, 7, 6, 40),
                new Date(122, 10, 7, 7, 22)), 18);
    }

    @Test
    public void testMaxFee() {
        // test max is not exceeded
        Assertions.assertEquals(TollCalculator.getTotalFee(
                new Car(), new Date(122, 10, 7, 7, 27),
                new Date(122, 10, 7, 8, 33),
                new Date(122, 10, 7, 15, 38),
                new Date(122, 10, 7, 16, 57),
                new Date(122, 10, 7, 18, 29)), 60);
    }
}