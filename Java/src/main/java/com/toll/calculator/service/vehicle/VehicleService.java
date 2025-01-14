package com.toll.calculator.service.vehicle;

import com.toll.calculator.dto.vehicle.VehicleDto;
import com.toll.calculator.dto.vehicle.VehicleResponseDto;

import java.util.List;

public interface VehicleService {
    VehicleResponseDto createVehicle(VehicleDto vehicleDto);
    VehicleResponseDto addTollDates(long id, List<String> tollDates);
    VehicleResponseDto getVehicle(long id);
    VehicleResponseDto updateVehicle(long id, VehicleDto vehicleDto);
    void deleteVehicle(long id);
    List<VehicleResponseDto> getAllVehicle();
}
