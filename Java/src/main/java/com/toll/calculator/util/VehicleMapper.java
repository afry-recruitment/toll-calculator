package com.toll.calculator.util;

import com.toll.calculator.dto.vehicle.VehicleResponseDto;
import com.toll.calculator.dto.vehicle.VehicleTypeResponseDto;
import com.toll.calculator.model.vehicle.VehicleEntity;
import com.toll.calculator.model.vehicle.VehicleType;

import java.util.List;

import static com.toll.calculator.util.DateUtil.getDatesString;

public class VehicleMapper {

    public static VehicleTypeResponseDto toVehicleTypeResponseDto(VehicleType vehicleType){
        if (vehicleType == null) {
            return null;
        }

        VehicleTypeResponseDto dto = new VehicleTypeResponseDto();
        dto.setId(vehicleType.getId());
        dto.setName(vehicleType.getName());
        dto.setTollFree(vehicleType.isTollFree());

        return dto;

    }

    public static VehicleResponseDto toVehicleResponseDto(VehicleEntity vehicle) {
        if (vehicle == null) {
            return null;
        }

        VehicleResponseDto dto = new VehicleResponseDto();
        dto.setVehicleId(vehicle.getId());
        dto.setRegNumber(vehicle.getRegNumber());
        dto.setVehicleType(vehicle.getVehicleType());
        dto.setTollDates(getDatesString(vehicle.getTollDates()));

        return dto;
    }

    public static List<VehicleResponseDto> toVehicleResponseDtoList(List<VehicleEntity> vehicles) {
        return vehicles.stream()
                .map(VehicleMapper::toVehicleResponseDto).toList();
    }

    public static List<VehicleTypeResponseDto> toVehicleResponseTypeDtoList(List<VehicleType> vehicleTypes) {
        return vehicleTypes.stream()
                .map(VehicleMapper::toVehicleTypeResponseDto).toList();
    }
}
