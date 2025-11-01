package com.assignment.toll_calculator.controllers;

import com.assignment.toll_calculator.models.response.InfoResponse;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.tags.Tag;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import static com.assignment.toll_calculator.utils.ConstantUtils.*;

/**
 * This is the rest controller class where the endpoints giving info about the application.
 */
@Tag(name = "Application", description = "Basic application info")
@RestController
@Slf4j
public class InfoController {

    @Value("${spring.application.name}")
    private String applicationName;

    @Operation(
            summary = "Get application info",
            description = "This endpoint gives back basic info about the application."
    )
    @ApiResponse(
            responseCode = "200",
            description = "Printed application info",
            content = @Content(
                    mediaType = MediaType.APPLICATION_JSON_VALUE, schema = @Schema(implementation = InfoResponse.class)
            )
    )
    @GetMapping(value = INFO_URL, produces = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<InfoResponse> appInfo() {
        log.debug("Received GET {} request", INFO_URL);

        InfoResponse infoResponse = new InfoResponse(applicationName);

        log.debug("Received GET {} request", INFO_URL);

        return new ResponseEntity<>(infoResponse, HttpStatus.OK);
    }

}
