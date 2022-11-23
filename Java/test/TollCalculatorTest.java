package test;


import static org.junit.jupiter.api.Assertions.*;
import java.util.Date;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import calculations.CalculateTotalFee;
import calculations.TollCalculator;
import typeOfVehicle.Car;
import typeOfVehicle.Motorbike;


public class TollCalculatorTest {
	Date dateStartTest = new Date();
	Date[] dateArrayTest = new Date[6];
	Motorbike motorbikeTest=new Motorbike();
	TollCalculator tollCalculator= new TollCalculator();;

	@BeforeEach
	void setUp() {
		dateStartTest.getTime();
	}

	@Test
	public void testGetTollFee() {	
		
		int x=tollCalculator.getTollFee(motorbikeTest,dateStartTest);
		assertEquals(0,x);
		
	}
}
