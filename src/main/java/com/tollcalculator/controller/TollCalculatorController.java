package com.tollcalculator.controller;

import com.tollcalculator.boObject.TollCalculatorRequest;
import com.tollcalculator.boObject.TollCalculatorResponse;
import com.tollcalculator.exceptions.InvalidParameterException;
import com.tollcalculator.service.TollCalculatorService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

/**
 * This Toll Fee controller has request mapping
 */
@RestController
@RequestMapping("/api/v1/tax-calculate")
public class TollCalculatorController {
    private static final Logger LOG = LoggerFactory.getLogger(TollCalculatorController.class);

    private final TollCalculatorService tollCalculatorService;

    public TollCalculatorController(TollCalculatorService tollCalculatorService) {
        this.tollCalculatorService = tollCalculatorService;
    }

    @PostMapping
    public ResponseEntity<TollCalculatorResponse> calculateToll(@RequestBody TollCalculatorRequest tollCalculatorRequest) throws InvalidParameterException {
        LOG.debug("Request received for toll fee calculation for City {}, Vehicle {} and dates {}", tollCalculatorRequest.getCity(), tollCalculatorRequest.getVehicle(), tollCalculatorRequest.getCheckInTime());
        tollCalculatorService.validateRequest(tollCalculatorRequest);
        TollCalculatorResponse result = tollCalculatorService.getTaxAmount(tollCalculatorRequest.getCity(), tollCalculatorRequest.getVehicle(), tollCalculatorRequest.getCheckInTime());
        return new ResponseEntity<>(result, HttpStatus.OK);
    }
}
