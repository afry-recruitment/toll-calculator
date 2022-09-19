package com.afry.test.controller;

import java.util.Date;

import com.afry.test.service.VehicleFactory;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import com.afry.test.model.TollEntryRequest;
import com.afry.test.model.TollEntryResponse;
import com.afry.test.model.Vehicle;
import com.afry.test.service.TollCalculatorService;

/**
 * TollController to provide Toll Gate API for Toll fee calculator 1.0 consumers
 */
@RestController
@RequestMapping(value = "/")
public class TollCalculatorController {
    private final Logger logger = LoggerFactory.getLogger(TollCalculatorController.class);
    @Autowired(required = true)
    private TollCalculatorService tollCalculatorService;
    @Autowired(required = true)
    private VehicleFactory vehicleFactory;

    /**
     * POST Method For The Toll Calculator
     *
     * @param tollEntryRequest
     * @return
     */
    @PostMapping(value = "/calculator")
    public ResponseEntity<TollEntryResponse> tollGate(@RequestBody TollEntryRequest tollEntryRequest) {
        // Validate input TollEntryRequest
        if (tollEntryRequest.getType().isEmpty()) {
            logger.info(String.format("BAD_REQUEST -TollEntryRequest - Vehicle Type Cannot be Empty"));
            return new ResponseEntity("TollEntryRequest -Vehicle Type Cannot be Empty", HttpStatus.BAD_REQUEST);
        }
        if (tollEntryRequest.getVehicleId().isEmpty()) {
            logger.info(String.format("BAD_REQUEST -TollEntryRequest - Vehicle ID Cannot be Empty"));
            return new ResponseEntity("TollEntryRequest -Vehicle ID Cannot be Empty", HttpStatus.BAD_REQUEST);
        }
        // Validate input for Vehicle_ID length
        if (tollEntryRequest.getVehicleId().length() != 8) {
            logger.info(String.format("BAD_REQUEST -TollEntryRequest - Invalid Vehicle ID - Vehicle Registration ID Should be 8 Characters"));
            return new ResponseEntity("TollEntryRequest -Invalid Vehicle ID - Vehicle Registration ID Should be 8 Characters", HttpStatus.BAD_REQUEST);
        }

        try {
            Vehicle vehicle = vehicleFactory.getVehicle(tollEntryRequest.getType());
            vehicle.setVehicleId(tollEntryRequest.getVehicleId());

            TollEntryResponse tollEntryResponse = new TollEntryResponse();
            tollEntryResponse.setVehicleId(vehicle.getVehicleId());
            tollEntryResponse.setType(vehicle.getType());

            if (tollCalculatorService.isTollFreeVehicle(vehicle)) {
                return new ResponseEntity(vehicle.getVehicleId() + " is Toll Free Vehicle", HttpStatus.OK);
            } else if (tollCalculatorService.isTollFreeDate(new Date())) {
                return new ResponseEntity("Today is Toll Free Day", HttpStatus.OK);
            } else {
                Integer tollFee = tollCalculatorService.calculateTollFee(vehicle);
                tollEntryResponse.setTollFee(tollFee);
                return new ResponseEntity(tollEntryResponse, HttpStatus.OK);
            }
        } catch (Exception e) {
            logger.info(String.format("INTERNAL_SERVER_ERROR -" + e.getMessage()));
            return new ResponseEntity("INTERNAL_SERVER_ERROR", HttpStatus.INTERNAL_SERVER_ERROR);
        }
    }
}

