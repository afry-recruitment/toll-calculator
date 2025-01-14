package com.toll.calculator;

import com.toll.calculator.dto.TollRequestWithVehicleType;
import com.toll.calculator.dto.TollResponse;
import com.toll.calculator.model.vehicle.VehicleType;
import com.toll.calculator.service.TollCalculatorService;
import org.junit.jupiter.api.Test;
import org.mockito.InjectMocks;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;

import java.util.Arrays;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

@SpringBootTest
@AutoConfigureMockMvc
class CalculatorApplicationTests {


	@Autowired
	@InjectMocks
	private TollCalculatorService tollCalculatorService;


	@Test
	public void testGetTollFee_WithTollVehicle() throws Exception {
		VehicleType mockVehicleType = new VehicleType();
		mockVehicleType.setTollFree(true);
		TollRequestWithVehicleType tollRequest = new TollRequestWithVehicleType();
		tollRequest.setVehicleTypeName("Car");
		tollRequest.setCityCode("GB");
		tollRequest.setTollDates(Arrays.asList(
				"01-01-2025 08:15:00",
				"01-01-2025 10:30:00",
				"02-01-2025 14:45:00",
				"03-01-2025 18:00:00"
		));

		TollResponse response = tollCalculatorService.getTollFee(tollRequest);

		assertNotNull(response);
		assertEquals(16.0d, response.getToll()); // Toll should be zero for toll-free vehicle
	}

	@Test
	public void testGetTollFee_WithTollFreeVehicle() throws Exception {
		VehicleType mockVehicleType = new VehicleType();
		mockVehicleType.setTollFree(true);
		TollRequestWithVehicleType tollRequest = new TollRequestWithVehicleType();
		tollRequest.setVehicleTypeName("Foreign");
		tollRequest.setCityCode("GB");
		tollRequest.setTollDates(Arrays.asList(
				"01-01-2025 08:15:00",
				"01-01-2025 10:30:00",
				"02-01-2025 14:45:00",
				"03-01-2025 18:00:00"
		));

		TollResponse response = tollCalculatorService.getTollFee(tollRequest);

		assertNotNull(response);
		assertEquals(0.0d, response.getToll()); // Toll should be zero for toll-free vehicle
	}

	@Test
	public void testGetTollFee_OnPublicHolidays() throws Exception {
		VehicleType mockVehicleType = new VehicleType();
		mockVehicleType.setTollFree(true);
		TollRequestWithVehicleType tollRequest = new TollRequestWithVehicleType();
		tollRequest.setVehicleTypeName("Foreign");
		tollRequest.setCityCode("GB");
		tollRequest.setTollDates(Arrays.asList(
				"01-01-2025 08:15:00",
				"06-01-2025 10:30:00",
				"18-04-2025 14:45:00",
				"20-04-2025 18:00:00"
		));

		TollResponse response = tollCalculatorService.getTollFee(tollRequest);

		assertNotNull(response);
		assertEquals(0.0d, response.getToll()); // Toll should be zero for toll-free vehicle
	}


}
