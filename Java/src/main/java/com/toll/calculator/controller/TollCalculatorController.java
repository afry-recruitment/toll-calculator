package com.toll.calculator.controller;

import com.toll.calculator.dto.TollRequestWithVehicleType;
import com.toll.calculator.dto.TollResponse;
import com.toll.calculator.dto.vehicle.VehicleTollRequest;
import com.toll.calculator.exception.TollException;
import com.toll.calculator.service.TollCalculatorService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.server.ResponseStatusException;

@RestController
@RequestMapping("/api/v1/toll")
@RequiredArgsConstructor
@Slf4j
public class TollCalculatorController {

    @Autowired
    private final TollCalculatorService tollCalculatorService;

    @PostMapping
    public ResponseEntity<TollResponse> getTax(@RequestBody VehicleTollRequest tollRequest) {
        try {
            return ResponseEntity.ok().body(tollCalculatorService.getTollFee(tollRequest));
        } catch (IllegalArgumentException ex) {
            log.error(ex.toString(), ex);
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        } catch (TollException ex) {
            log.error(ex.toString(), ex);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }

    @PostMapping("/vehicle-type")
    public ResponseEntity<TollResponse> handleTollRequest(@RequestBody TollRequestWithVehicleType tollRequest) {
        try {
            return ResponseEntity.ok().body(tollCalculatorService.getTollFee(tollRequest));
        } catch (IllegalArgumentException ex) {
            log.error(ex.toString(), ex);
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        } catch (TollException ex) {
            log.error(ex.toString(), ex);
            throw new ResponseStatusException(HttpStatus.INTERNAL_SERVER_ERROR, ex.getMessage());
        }
    }
}
