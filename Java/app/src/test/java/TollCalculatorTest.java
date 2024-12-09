
import org.junit.jupiter.api.Test;

import com.tollcalculator.Car;
import com.tollcalculator.Motorbike;
import com.tollcalculator.TollCalculator;
import com.tollcalculator.Vehicle;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

import java.time.LocalDate;
import java.time.ZoneId;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

import org.junit.jupiter.api.Assertions;

public class TollCalculatorTest {

	@Test
	public void ensureCorrectCarTollForDay(){
		
		// Friday with no holiday
		Date[] fridayDates = {
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 6, 23).getTime(), // 8
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 7, 26).getTime(), // 18
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 8, 12).getTime(), // 0
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 9, 11).getTime(), // 8
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 9, 12).getTime(), // 0
        };

		// Saturday
		Date[] saturdayDates = {
			new GregorianCalendar(2024, Calendar.DECEMBER, 7, 6, 23).getTime(), // 0
			new GregorianCalendar(2024, Calendar.DECEMBER, 7, 7, 26).getTime(), // 0
			new GregorianCalendar(2024, Calendar.DECEMBER, 7, 8, 12).getTime(), // 0
        };

		// Christmas
		Date[] christmasDates = {
			new GregorianCalendar(2024, Calendar.DECEMBER, 25, 6, 23).getTime(), // 0
			new GregorianCalendar(2024, Calendar.DECEMBER, 25, 7, 26).getTime(), // 0
			new GregorianCalendar(2024, Calendar.DECEMBER, 25, 8, 12).getTime(), // 0
			new GregorianCalendar(2024, Calendar.DECEMBER, 25, 14, 12).getTime(), // 0
        };

		// Friday with no holiday
		Date[] tollCostOverflowDates = {
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 6, 0).getTime(), 
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 7, 1).getTime(), 
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 8, 2).getTime(), 
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 9, 3).getTime(), 
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 10, 4).getTime(),
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 11, 5).getTime(),
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 12, 6).getTime(),
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 13, 7).getTime(),
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 14, 8).getTime(),
		};

		Vehicle car = new Car();
		TollCalculator tollCalculator = new TollCalculator();

		assertEquals(tollCalculator.getTollFee(car, fridayDates), 34);
		assertEquals(tollCalculator.getTollFee(car, saturdayDates), 0);
		assertEquals(tollCalculator.getTollFee(car, christmasDates), 0);
		assertEquals(tollCalculator.getTollFee(car, tollCostOverflowDates), 60);
	}

    @Test
	public void ensureErrorThrownOnInvalidInput(){
		// Invalid input
		Date[] invalidInputDates = {
			new GregorianCalendar(2024, Calendar.DECEMBER, 25, 6, 23).getTime(),
			new GregorianCalendar(2024, Calendar.DECEMBER, 14, 6, 23).getTime(),
        };

		Vehicle car = new Car();
		TollCalculator tollCalculator = new TollCalculator();

		assertThrows(Error.class,() -> tollCalculator.getTollFee(car, invalidInputDates));
	}

	@Test
	public void ensureCorrectMotorbikeTollForDay(){
		
		// Friday with no holiday
		Date[] dates = {
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 8, 12).getTime(), // 0
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 9, 11).getTime(), // 8
			new GregorianCalendar(2024, Calendar.DECEMBER, 6, 9, 12).getTime(), // 0
        };

		Vehicle motorbike = new Motorbike();
		TollCalculator tollCalculator = new TollCalculator();

		int totalFee = tollCalculator.getTollFee(motorbike, dates);
		assertEquals(totalFee, 0);
	}

	@Test
	public void ensureFreeWeekend(){
		TollCalculator tollCalculator = new TollCalculator();

		Vehicle car = new Car();
		Vehicle motorbike = new Motorbike();

		Date saturday = new GregorianCalendar(2024, Calendar.DECEMBER, 7).getTime();
		int carTollFeeSaturday = tollCalculator.getTollFee(saturday, car);
		int motorbikeTollFeeSaturday = tollCalculator.getTollFee(saturday, motorbike);
		
 		Date sunday = new GregorianCalendar(2024, Calendar.DECEMBER, 8).getTime();
		int carTollFeeSunday = tollCalculator.getTollFee(sunday, car);
		int motorbikeTollFeeSunday = tollCalculator.getTollFee(sunday, motorbike);


		assertEquals(carTollFeeSaturday, 0);
		assertEquals(motorbikeTollFeeSaturday, 0);
		assertEquals(carTollFeeSunday, 0);
		assertEquals(motorbikeTollFeeSunday, 0);
	}

	@Test
	public void ensureTollFreeVehicle(){
		TollCalculator tollCalculator = new TollCalculator();

		Vehicle car = new Car();
		Vehicle motorbike = new Motorbike();

		boolean isCarTollFree = tollCalculator.isTollFreeVehicle(car);
		boolean isMotorbikeTollFree = tollCalculator.isTollFreeVehicle(motorbike);
		
		assertEquals(isCarTollFree, false);
		assertEquals(isMotorbikeTollFree, true);
	}

	@Test
	public void ensureFreeHoliday(){
		TollCalculator tollCalculator = new TollCalculator();

		Vehicle car = new Car();
		Vehicle motorbike = new Motorbike();

		Date christmas = new GregorianCalendar(2024, Calendar.DECEMBER, 25).getTime();

		int carTollFee = tollCalculator.getTollFee(christmas, car);
		int motorbikeTollFee = tollCalculator.getTollFee(christmas, motorbike);

		assertEquals(carTollFee, 0);
		assertEquals(motorbikeTollFee, 0);
	}
}
