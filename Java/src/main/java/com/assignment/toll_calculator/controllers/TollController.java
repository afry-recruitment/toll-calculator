package com.assignment.toll_calculator.controllers;

import com.assignment.toll_calculator.models.TollResponse;
import com.assignment.toll_calculator.services.TollService;
import com.assignment.toll_calculator.utils.DateUtils;
import com.assignment.toll_calculator.utils.VehicleTypeUtils;
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

    private static final String DATES_REQUEST_PARAM_VALUE_DELIMITER = ",";

    private final TollService tollService;

    @Autowired
    public TollController(TollService tollService) {
        this.tollService = tollService;
    }

	@GetMapping(value = FEE_URL, produces = MediaType.APPLICATION_JSON_VALUE)
	public ResponseEntity<TollResponse> calculateFee(
            @RequestParam(value = VEHICLE_TYPE_REQUEST_PARAM) String vehicleType,
            @RequestParam(value = DATES_REQUEST_PARAM) String dates) {

		return new ResponseEntity<>(
                tollService.calculateFee(
                        VehicleTypeUtils.parse(vehicleType),
                        new ArrayList<>(Arrays.asList(dates.split(DATES_REQUEST_PARAM_VALUE_DELIMITER)))
                                .stream()
                                .map(date -> DateUtils.convert(date, DateUtils.DATE_TIME_PATTERN))
                                .toList()
                ),
                HttpStatus.OK
        );

	}

}