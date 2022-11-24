package test;

import static org.junit.Assert.assertNotEquals;
import static org.junit.jupiter.api.Assertions.assertEquals;
import java.util.Date;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import calculations.IsTollFreeDate;

public class IsTollFreeDateTest {

	IsTollFreeDate isTollFreeDateTest;
	Date dateChristmasDay;
	Date dateSaturday;
	Date workingDay;


	@Test
	@DisplayName("Is it toll free day")
	public void testIsTollFreeDate() {
		isTollFreeDateTest = new IsTollFreeDate();
		dateChristmasDay = new Date();
		 dateSaturday = new Date();
		 workingDay= new Date();
		
		
		dateSaturday.setMonth(11);
		dateSaturday.setDate(24);

		// Christmas Day
		dateChristmasDay.setMonth(11);
		dateChristmasDay.setDate(26);

		// working day
		workingDay.setMonth(10);
		workingDay.setDate(24);
		
		
		System.out.println(dateSaturday);
		System.out.println(dateChristmasDay);
		System.out.println(workingDay);
		assertEquals(true, isTollFreeDateTest.isTollFreeDate(dateSaturday));
		assertEquals(true, isTollFreeDateTest.isTollFreeDate(dateChristmasDay));
		assertNotEquals(true, isTollFreeDateTest.isTollFreeDate(workingDay));

	}
}
