package com.toll.calculator.service.vehicletype;

import com.toll.calculator.dto.vehicle.VehicleTypeDto;
import com.toll.calculator.dto.vehicle.VehicleTypeResponseDto;

import java.util.List;

public interface VehicleTypeService {
    VehicleTypeResponseDto createVehicleType(VehicleTypeDto vehicleTypeDto);
    VehicleTypeResponseDto getVehicleType(long id);
    VehicleTypeResponseDto updateVehicleType(long id, VehicleTypeDto vehicleTypeDto);
    void deleteVehicleType(long id);
    List<VehicleTypeResponseDto> getAllVehicleType();
}
