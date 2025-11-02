package com.assignment.toll_calculator.controllers;

import com.assignment.toll_calculator.models.internal.VehicleType;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.Arrays;
import java.util.List;

import static com.assignment.toll_calculator.utils.ConstantUtils.*;

/**
 * This is the rest controller class where the application endpoints regarding the vehicles are defined.
 */
@Tag(name = "Vehicle", description = "Vehicle operations")
@RestController
@Slf4j
public class VehicleTypeController {

    @Operation(
            summary = "Get vehicle types",
            description = "By this endpoint, you can list the available vehicle types."
    )
    @ApiResponse(
            responseCode = "200",
            description = "Listed vehicle types",
            content = @Content(
                    mediaType = MediaType.APPLICATION_JSON_VALUE, schema = @Schema(implementation = List.class)
            )
    )
    @GetMapping(value = VEHICLE_TYPES_URL, produces = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<List<String>> fetchVehicleTypes() {
        log.debug("Received GET {} request", VEHICLE_TYPES_URL);

        List<String> vehicleTypes = Arrays.stream(VehicleType.values()).map(VehicleType::getType).toList();

        log.debug("Finished GET {} request", VEHICLE_TYPES_URL);

        return new ResponseEntity<>(vehicleTypes, HttpStatus.OK);
    }

}