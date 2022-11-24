package test;


import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotEquals;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import calculations.CalculateTotalFee;

public class CalculateTotalFeeTest {

	CalculateTotalFee calculateTotalFeeTest;

	Class<CalculateTotalFee>c;
	Object obj;
	Method m;

	@BeforeEach
	void setUp() throws NoSuchMethodException, SecurityException, InstantiationException, IllegalAccessException {
		 c = CalculateTotalFee.class;
		calculateTotalFeeTest = new CalculateTotalFee();
		obj = c.newInstance();
	}

	// test fee based is or is not rush hour
	@Test
	@DisplayName("Is or is not rush hour")
	public void testReturnFee() throws IllegalAccessException, IllegalArgumentException, InvocationTargetException,
			NoSuchMethodException, SecurityException {
		m = c.getDeclaredMethod("returnFee", new Class[] { double.class });
		m.setAccessible(true);

		// if it is 8 am it is rush hover, fee is 18
		assertEquals(18, m.invoke(obj, 8));
		// if it is 13 am it is not rush hover, fee is 8
		assertEquals(8, m.invoke(obj, 13));
		// if it is 13o'clock fee should be 8 not 18sek
		assertNotEquals(18, m.invoke(obj, 13));
	}

	// test is fee=60 if total fee based on entries is bigger then 60sek
	@Test
	@DisplayName("Is fee bigger or smaller then 60")
	public void testCheckIsTotalFeeBigerThanMaximum() throws IllegalAccessException, IllegalArgumentException,
			InvocationTargetException, NoSuchMethodException, SecurityException {

		m = c.getDeclaredMethod("checkIsTotalFeeBigerThanMaximum", new Class[] { int.class });
		m.setAccessible(true);
		assertEquals(55, m.invoke(obj, 55));

		assertEquals(60, m.invoke(obj, 65));
	}
	
}
