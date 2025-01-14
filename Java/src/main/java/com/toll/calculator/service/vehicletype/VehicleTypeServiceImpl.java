package com.toll.calculator.service.vehicletype;

import com.toll.calculator.dto.vehicle.VehicleTypeDto;
import com.toll.calculator.dto.vehicle.VehicleTypeResponseDto;
import com.toll.calculator.exception.ResourceNotFoundException;
import com.toll.calculator.model.vehicle.VehicleType;
import com.toll.calculator.repository.VehicleTypeRepository;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

import static com.toll.calculator.util.VehicleMapper.toVehicleResponseTypeDtoList;
import static com.toll.calculator.util.VehicleMapper.toVehicleTypeResponseDto;

@Service
@Slf4j
public class VehicleTypeServiceImpl implements VehicleTypeService{

    @Autowired
    private VehicleTypeRepository vehicleTypeRepository;

    @Override
    public VehicleTypeResponseDto createVehicleType(VehicleTypeDto vehicleTypeDto) {
        VehicleType vehicleType = new VehicleType();
        vehicleType.setName(vehicleTypeDto.getName());
        vehicleType.setTollFree(vehicleTypeDto.isTollFree());
        return toVehicleTypeResponseDto(vehicleTypeRepository.save(vehicleType));
    }

    @Override
    public VehicleTypeResponseDto getVehicleType(long id) {
        VehicleType vehicleType = vehicleTypeRepository.findById(id).orElseThrow(()-> new ResourceNotFoundException("Vehicle type not found with id: " + id));
        return toVehicleTypeResponseDto(vehicleType);
    }

    @Override
    public VehicleTypeResponseDto updateVehicleType(long id, VehicleTypeDto vehicleTypeDto) {
        VehicleType vehicleType = vehicleTypeRepository.findById(id).orElseThrow(()-> new ResourceNotFoundException("Vehicle type not found with id: " + id));
        vehicleType.setName(vehicleTypeDto.getName());
        vehicleType.setTollFree(vehicleTypeDto.isTollFree());
        return toVehicleTypeResponseDto(vehicleType);
    }

    @Override
    public void deleteVehicleType(long id) {
        vehicleTypeRepository.deleteById(id);
    }

    @Override
    public List<VehicleTypeResponseDto> getAllVehicleType() {
        return toVehicleResponseTypeDtoList(vehicleTypeRepository.findAll());
    }
}
