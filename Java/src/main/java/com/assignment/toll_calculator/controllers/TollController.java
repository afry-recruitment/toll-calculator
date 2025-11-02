package com.assignment.toll_calculator.controllers;

import com.assignment.toll_calculator.models.response.ErrorResponse;
import com.assignment.toll_calculator.models.response.TollFeeResponse;
import com.assignment.toll_calculator.services.TollService;
import com.assignment.toll_calculator.utils.DateUtils;
import com.assignment.toll_calculator.utils.VehicleTypeUtils;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.constraints.NotEmpty;
import lombok.extern.slf4j.Slf4j;
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
@Tag(name = "Toll", description = "Toll operations")
@RestController
@Validated
@Slf4j
public class TollController {

    private final TollService tollService;

    @Autowired
    public TollController(TollService tollService) {
        this.tollService = tollService;
    }

    @Operation(
            summary = "Get total fee",
            description = "By this endpoint, you can calculate the total fee based on the different parameters."
    )
    @ApiResponse(
            responseCode = "200",
            description = "Calculated total fee",
            content = @Content(
                    mediaType = MediaType.APPLICATION_JSON_VALUE,
                    schema = @Schema(implementation = TollFeeResponse.class)
            )
    )
    @ApiResponse(
            responseCode = "400",
            description = "Bad request occurred",
            content = @Content(
                    mediaType = MediaType.APPLICATION_JSON_VALUE, schema = @Schema(implementation = ErrorResponse.class)
            )
    )
    @ApiResponse(
            responseCode = "500",
            description = "Internal server error occurred",
            content = @Content(
                    mediaType = MediaType.APPLICATION_JSON_VALUE, schema = @Schema(implementation = ErrorResponse.class)
            )
    )
	@GetMapping(value = FEE_URL, produces = MediaType.APPLICATION_JSON_VALUE)
	public ResponseEntity<TollFeeResponse> calculateFee(
            @NotEmpty(message = INVALID_VEHICLE_TYPE_ERROR_MESSAGE)
            @RequestParam(value = VEHICLE_TYPE_REQUEST_PARAM) String vehicleType,
            @NotEmpty(message = INVALID_DATES_ERROR_MESSAGE)
            @RequestParam(value = DATES_REQUEST_PARAM) String dates) {

        log.debug("Received GET {} request with vehicle type {} and dates {}", FEE_URL, vehicleType, dates);

        TollFeeResponse tollFeeResponse = new TollFeeResponse(
                tollService.calculateFee(
                        VehicleTypeUtils.parse(vehicleType),
                        new ArrayList<>(Arrays.asList(dates.split(DEFAULT_VALUE_DELIMITER)))
                                .stream()
                                .map(date -> DateUtils.convert(date, DATE_TIME_PATTERN))
                                .toList()
                )
        );

        log.debug("Finished GET {} request with vehicle type {} and dates {}", FEE_URL, vehicleType, dates);

        return new ResponseEntity<>(tollFeeResponse, HttpStatus.OK);

	}

}