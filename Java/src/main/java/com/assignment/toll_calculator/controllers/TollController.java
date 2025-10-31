package com.assignment.toll_calculator.controllers;

import com.assignment.toll_calculator.errors.ApiException;
import com.assignment.toll_calculator.models.TollResponse;
import com.assignment.toll_calculator.models.VehicleType;
import com.assignment.toll_calculator.services.TollService;
import com.assignment.toll_calculator.utils.DateUtils;
import com.assignment.toll_calculator.utils.VehicleTypeUtils;
import jakarta.validation.constraints.NotEmpty;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.annotation.Validated;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import java.util.ArrayList;
import java.util.Arrays;

/**
 * This is the rest controller class where the application endpoints regarding the toll fee calculation are defined.
 */
@RestController
@Validated
public class TollController {

    public static final String FEE_URL = "/fee";

    public static final String VEHICLE_TYPE_REQUEST_PARAM = "vehicle_type";
    public static final String DATES_REQUEST_PARAM = "dates";

    public static final String DATES_REQUEST_PARAM_VALUE_DELIMITER = ",";

    private final TollService tollService;

    @Autowired
    public TollController(TollService tollService) {
        this.tollService = tollService;
    }

	@GetMapping(value = FEE_URL, produces = MediaType.APPLICATION_JSON_VALUE)
	public ResponseEntity<TollResponse> calculateFee(
            @NotEmpty(message = "The vehicle type is required!")
            @RequestParam(value = VEHICLE_TYPE_REQUEST_PARAM) String vehicleType,
            @NotEmpty(message = "At least one date is required!")
            @RequestParam(value = DATES_REQUEST_PARAM) String dates) {

        VehicleType parsedVehicleType;
        try {
            parsedVehicleType = VehicleTypeUtils.parse(vehicleType);
        } catch (IllegalArgumentException exception) {
            throw new ApiException(exception, HttpStatus.BAD_REQUEST);
        }

		return new ResponseEntity<>(
                new TollResponse(
                        tollService.calculateFee(
                                parsedVehicleType,
                                new ArrayList<>(Arrays.asList(dates.split(DATES_REQUEST_PARAM_VALUE_DELIMITER)))
                                        .stream()
                                        .map(date -> DateUtils.convert(date, DateUtils.DATE_TIME_PATTERN))
                                        .toList()
                        )
                ),
                HttpStatus.OK
        );

	}

}