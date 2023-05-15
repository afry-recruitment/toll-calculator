package com.tollcalculator.controller;

import com.tollcalculator.boObject.TollCalculatorRequest;
import com.tollcalculator.boObject.TollCalculatorResponse;
import com.tollcalculator.boObject.VehicleType;
import com.tollcalculator.exceptions.InvalidParameterException;
import com.tollcalculator.service.TollCalculatorService;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import static org.assertj.core.api.Assertions.assertThat;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertThrows;

@ExtendWith(MockitoExtension.class)
public class TollCalculatorControllerTest {

    public static final String CAR = "Car";
    public static final String GOTHENBURG = "Gothenburg";
    public static final String SHIP = "Ship";
    public static final String PUNE = "Pune";
    @InjectMocks
    private TollCalculatorController tollController;

    @Mock
    private TollCalculatorService tollCalculatorController;

    @Test
    public void calculateTollForValidInputTest() throws Exception {
        final String CAR = "Car";
        TollCalculatorRequest tollCalculatorRequest = buildTollRequest(CAR, GOTHENBURG);
        TollCalculatorResponse tollCalculatorResponse = TollCalculatorResponse.builder().tollFeeAmount(new BigDecimal(10)).build();
        Mockito.when(tollCalculatorController.getTollFeeAmount(tollCalculatorRequest.getCity(), tollCalculatorRequest.getVehicle(), tollCalculatorRequest.getCheckInTime())).thenReturn(tollCalculatorResponse);
        ResponseEntity<TollCalculatorResponse> response = tollController.calculateToll(tollCalculatorRequest);
        assertThat(response).isNotNull();
        assertThat(response.getStatusCode()).isEqualTo(HttpStatus.OK);
        assertThat(response.getBody().getTollFeeAmount()).isEqualTo(new BigDecimal(10));
    }

    @Test
    public void invalidInputVehicleTypeTest() throws InvalidParameterException {
        Throwable exception = assertThrows(InvalidParameterException.class, () -> {
            TollCalculatorRequest tollCalculatorRequest = buildTollRequest(SHIP, GOTHENBURG);
            Mockito.doThrow(new InvalidParameterException("Invalid Vehicle")).when(tollCalculatorController).validateRequest(tollCalculatorRequest);
            tollController.calculateToll(tollCalculatorRequest);
        });
        assertEquals("Invalid Vehicle", exception.getMessage());
    }

    @Test
    public void invalidInputCityTypeTest() throws InvalidParameterException {
        Throwable exception = assertThrows(InvalidParameterException.class, () -> {
            TollCalculatorRequest tollCalculatorRequest = buildTollRequest(CAR, PUNE);
            Mockito.doThrow(new InvalidParameterException("Invalid City")).when(tollCalculatorController).validateRequest(tollCalculatorRequest);
            tollController.calculateToll(tollCalculatorRequest);
        });
        assertEquals("Invalid City", exception.getMessage());
    }

    private TollCalculatorRequest buildTollRequest(String vehicleType, String cityName) {
        VehicleType vehicle = new VehicleType();
        vehicle.setType(vehicleType);
        List<Date> dateList = new ArrayList<>();
        dateList.add(new Date());
        TollCalculatorRequest request = new TollCalculatorRequest();
        request.setCity(cityName);
        request.setVehicle(vehicle);
        request.setCheckInTime(dateList);
        return request;
    }
}
