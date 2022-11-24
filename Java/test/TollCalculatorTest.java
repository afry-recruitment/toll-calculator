package test;

import static org.junit.jupiter.api.Assertions.*;
import java.util.Date;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import calculations.TollCalculator;
import typeOfVehicle.Car;
import typeOfVehicle.Motorbike;

public class TollCalculatorTest {

	Date dateSaturday;
	Date workingDay;
	Motorbike motorbikeTest;
	TollCalculator tollCalculatorTest = new TollCalculator();
	Car carTest;

	@BeforeEach
	void setUp() {
		dateSaturday = new Date();
		workingDay = new Date();
		motorbikeTest = new Motorbike();
		carTest = new Car();

		tollCalculatorTest = new TollCalculator();

		// working day
		workingDay.setMonth(10);
		workingDay.setDate(24);

		// saturday
		dateSaturday.setMonth(11);
		dateSaturday.setDate(24);
	}

	// if it is not special car but it is weekend
	@Test
	@DisplayName("Car's fee on the saturday")
	public void testGetTollFeeForNoSpecialVehiclesButWeekend() {
		assertEquals(0, tollCalculatorTest.getTollFee(carTest, dateSaturday));
	}

	// if vehicle is in the enum:TollFreeVehicles
	@Test
	@DisplayName("This is special vehicle from enum. Fee muste be 0")
	public void testGetTollFeeForSpecialVehicles() {
		assertEquals(0, tollCalculatorTest.getTollFee(motorbikeTest, workingDay));

	}

}
