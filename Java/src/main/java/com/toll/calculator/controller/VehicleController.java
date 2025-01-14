package com.toll.calculator.controller;

import com.toll.calculator.dto.vehicle.VehicleDto;
import com.toll.calculator.dto.vehicle.VehicleResponseDto;
import com.toll.calculator.service.vehicle.VehicleService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/v1/vehicle")
@RequiredArgsConstructor
@Slf4j
public class VehicleController {
    @Autowired
    VehicleService vehicleService;

    @PostMapping
    public ResponseEntity<VehicleResponseDto> createVehicle(@RequestBody VehicleDto vehicleDto) {
        return new ResponseEntity<>(vehicleService.createVehicle(vehicleDto), HttpStatus.CREATED);
    }

    @PostMapping("/{id}/toll-dates")
    public ResponseEntity<VehicleResponseDto> addTollDates(
            @PathVariable long id,
            @RequestBody List<String> tollDates
    ) {
        VehicleResponseDto updatedVehicle = vehicleService.addTollDates(id, tollDates);
        return ResponseEntity.ok(updatedVehicle);
    }

    @GetMapping
    public ResponseEntity<List<VehicleResponseDto>> getAllVehicle() {
        return new ResponseEntity<>(vehicleService.getAllVehicle(), HttpStatus.OK);
    }

    @PutMapping("/{id}")
    public ResponseEntity<VehicleResponseDto> updateVehicle(@RequestBody VehicleDto vehicleDto, @PathVariable(value = "id", required = true) long id) {
        return new ResponseEntity<>(vehicleService.updateVehicle(id, vehicleDto), HttpStatus.OK);
    }

    @GetMapping("/{id}")
    public ResponseEntity<VehicleResponseDto> getVehicle(@PathVariable(value = "id", required = true) long id) {
        return new ResponseEntity<>(vehicleService.getVehicle(id), HttpStatus.OK);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteVehicle(@PathVariable(value = "id", required = true) long id) {
        vehicleService.deleteVehicle(id);
        return new ResponseEntity<>(HttpStatus.NO_CONTENT);
    }
}
