package com.tollcalculator.controller;

import com.tollcalculator.boObject.TollCalculatorRequest;
import com.tollcalculator.boObject.TollCalculatorResponse;
import com.tollcalculator.exceptions.InvalidParameterException;
import com.tollcalculator.service.TollCalculatorService;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;

/**
 * This Toll Fee controller has request mapping
 */
@RestController
@RequestMapping("/api/v1/tax-calculate")
public class TollCalculatorController {

    private final TollCalculatorService tollCalculatorService;

    public TollCalculatorController(TollCalculatorService tollCalculatorService) {
        this.tollCalculatorService = tollCalculatorService;
    }

    @PostMapping
    public ResponseEntity<TollCalculatorResponse> calculateToll(@RequestBody TollCalculatorRequest tollCalculatorRequest) throws InvalidParameterException {
        tollCalculatorService.validateRequest(tollCalculatorRequest);
        TollCalculatorResponse result = tollCalculatorService.getTaxAmount(tollCalculatorRequest.getCity(), tollCalculatorRequest.getVehicle(), tollCalculatorRequest.getCheckInTime());
        return new ResponseEntity<>(result, HttpStatus.OK);
    }
}
