package test;


import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotEquals;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.Date;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import calculations.CalculateTotalFee;


public class CalculateTotalFeeTest {

	// Calendar dateNowTest = Calendar.getInstance();
	Date dateStartTest = new Date();
	Date[] dateArrayTest = new Date[6];

	CalculateTotalFee calculateTotalFee;

	Class<CalculateTotalFee> c = CalculateTotalFee.class;
	Object obj;
	Method m;

	@BeforeEach
	void setUp() throws NoSuchMethodException, SecurityException, InstantiationException, IllegalAccessException {
		calculateTotalFee = new CalculateTotalFee();
		dateStartTest.getTime();		
		obj = c.newInstance();

	}

	// test fee based is or is not rush hour
	@Test
	public void testReturnFee() throws IllegalAccessException, IllegalArgumentException, InvocationTargetException,
			NoSuchMethodException, SecurityException {
		m = c.getDeclaredMethod("returnFee", new Class[] { double.class });
		m.setAccessible(true);

		// if it is 8 am it is rush hover, fee is 18
		assertEquals(18, m.invoke(obj, 8));
		// if it is 13 am it is not rush hover, fee is 8
		assertEquals(8, m.invoke(obj, 13));
		// if it is 13o'clock fee shoult be 8 not 18sek
		assertNotEquals(18, m.invoke(obj, 13));
	}

	//test is fee=60 if total fee based on entries is bigger then 60sek
	@Test
	public void testCheckIsTotalFeeBigerThanMaximum() throws IllegalAccessException, IllegalArgumentException,
			InvocationTargetException, NoSuchMethodException, SecurityException {

		m = c.getDeclaredMethod("checkIsTotalFeeBigerThanMaximum", new Class[] { int.class });
		m.setAccessible(true);
		assertEquals(55, m.invoke(obj, 55));

		assertEquals(60, m.invoke(obj, 65));

	}

	
}
