package com.toll.calculator.controller;

import com.toll.calculator.dto.vehicle.VehicleTypeDto;
import com.toll.calculator.dto.vehicle.VehicleTypeResponseDto;
import com.toll.calculator.service.vehicletype.VehicleTypeService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/v1/vehicle-type")
@RequiredArgsConstructor
@Slf4j
public class VehicleTypeController {

    @Autowired
    VehicleTypeService vehicleTypeService;

    @PostMapping
    public ResponseEntity<VehicleTypeResponseDto> createVehicle(@RequestBody VehicleTypeDto vehicleTypeDto) {
        return new ResponseEntity<>(vehicleTypeService.createVehicleType(vehicleTypeDto), HttpStatus.CREATED);
    }

    @GetMapping
    public ResponseEntity<List<VehicleTypeResponseDto>> getAllVehicle() {
        return new ResponseEntity<>(vehicleTypeService.getAllVehicleType(), HttpStatus.OK);
    }

    @PutMapping("/{id}")
    public ResponseEntity<VehicleTypeResponseDto> updateVehicle(@RequestBody VehicleTypeDto vehicleTypeDto, @PathVariable(value = "id", required = true) long id) {
        return new ResponseEntity<>(vehicleTypeService.updateVehicleType(id, vehicleTypeDto), HttpStatus.OK);
    }

    @GetMapping("/{id}")
    public ResponseEntity<VehicleTypeResponseDto> getVehicle(@PathVariable(value = "id", required = true) long id) {
        return new ResponseEntity<>(vehicleTypeService.getVehicleType(id), HttpStatus.OK);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteVehicle(@PathVariable(value = "id", required = true) long id) {
        vehicleTypeService.deleteVehicleType(id);
        return new ResponseEntity<>(HttpStatus.NO_CONTENT);
    }
}
