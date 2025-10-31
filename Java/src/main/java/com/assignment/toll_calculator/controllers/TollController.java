package com.assignment.toll_calculator.controllers;

import com.assignment.toll_calculator.models.TollResponse;
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

import static com.assignment.toll_calculator.utils.ConstantUtils.*;

/**
 * This is the rest controller class where the application endpoints regarding the toll fee calculation are defined.
 */
@RestController
@Validated
public class TollController {

    private final TollService tollService;

    @Autowired
    public TollController(TollService tollService) {
        this.tollService = tollService;
    }

	@GetMapping(value = FEE_URL, produces = MediaType.APPLICATION_JSON_VALUE)
	public ResponseEntity<TollResponse> calculateFee(
            @NotEmpty(message = INVALID_VEHICLE_TYPE_ERROR_MESSAGE)
            @RequestParam(value = VEHICLE_TYPE_REQUEST_PARAM) String vehicleType,
            @NotEmpty(message = INVALID_DATES_ERROR_MESSAGE)
            @RequestParam(value = DATES_REQUEST_PARAM) String dates) {

		return new ResponseEntity<>(
                new TollResponse(
                        tollService.calculateFee(
                                VehicleTypeUtils.parse(vehicleType),
                                new ArrayList<>(Arrays.asList(dates.split(DATES_REQUEST_PARAM_VALUE_DELIMITER)))
                                        .stream()
                                        .map(date -> DateUtils.convert(date, DATE_TIME_PATTERN))
                                        .toList()
                        )
                ),
                HttpStatus.OK
        );

	}

}