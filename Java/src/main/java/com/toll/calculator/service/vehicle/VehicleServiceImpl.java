package com.toll.calculator.service.vehicle;

import com.toll.calculator.dto.vehicle.VehicleDto;
import com.toll.calculator.dto.vehicle.VehicleResponseDto;
import com.toll.calculator.exception.ResourceNotFoundException;
import com.toll.calculator.exception.TollException;
import com.toll.calculator.model.vehicle.VehicleEntity;
import com.toll.calculator.model.vehicle.VehicleType;
import com.toll.calculator.repository.VehicleRepository;
import com.toll.calculator.repository.VehicleTypeRepository;
import com.toll.calculator.util.DateUtil;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.Date;
import java.util.List;

import static com.toll.calculator.util.VehicleMapper.toVehicleResponseDto;
import static com.toll.calculator.util.VehicleMapper.toVehicleResponseDtoList;

@Service
@Slf4j
public class VehicleServiceImpl implements VehicleService{

    @Autowired
    private VehicleRepository vehicleRepository;

    @Autowired
    private VehicleTypeRepository vehicleTypeRepository;

    @Override
    public VehicleResponseDto createVehicle(VehicleDto vehicleDto) {
        VehicleType vehicleType = vehicleTypeRepository.findById(vehicleDto.getVehicleTypeId()).orElseThrow(()-> new ResourceNotFoundException("Vehicle type not found with id: " + vehicleDto.getVehicleTypeId()));
        VehicleEntity vehicleEntity = new VehicleEntity();
        vehicleEntity.setRegNumber(vehicleDto.getRegNumber());
        vehicleEntity.setVehicleType(vehicleType);
        vehicleEntity.setTollDates(vehicleDto.getTollDates());
        return toVehicleResponseDto(this.vehicleRepository.save(vehicleEntity));
    }

    @Override
    public VehicleResponseDto addTollDates(long id, List<String> tollDates) {
        VehicleEntity vehicle = this.vehicleRepository.findById(id).orElseThrow(()-> new ResourceNotFoundException("Vehicle not found with id: " + id));
        List<Date> dates = vehicle.getTollDates();
        dates.addAll(tollDates.stream().map(dateString -> {
            try {
                return DateUtil.getDateFromString(dateString);
            } catch (TollException e) {
                throw new RuntimeException(e);
            }
        }).toList());
        vehicle.setTollDates(dates);
        return toVehicleResponseDto(vehicleRepository.save(vehicle));
    }

    @Override
    public VehicleResponseDto getVehicle(long id) {
        VehicleEntity vehicle = vehicleRepository.findById(id).orElseThrow(()-> new ResourceNotFoundException("Vehicle not found with id: " + id));
        return toVehicleResponseDto(vehicle);
    }

    @Override
    public VehicleResponseDto updateVehicle(long id, VehicleDto vehicleDto) {
        VehicleEntity vehicle = vehicleRepository.findById(id).orElseThrow(()-> new ResourceNotFoundException("Vehicle not found with id: " + id));
        VehicleType vehicleType = vehicleTypeRepository.findById(vehicleDto.getVehicleTypeId()).orElseThrow(()-> new ResourceNotFoundException("Vehicle type not found with id: " + vehicleDto.getVehicleTypeId()));
        vehicle.setVehicleType(vehicleType);
        vehicle.setRegNumber(vehicleDto.getRegNumber());
        return toVehicleResponseDto(vehicleRepository.save(vehicle));
    }

    @Override
    public void deleteVehicle(long id) {
        vehicleRepository.deleteById(id);
    }

    @Override
    public List<VehicleResponseDto> getAllVehicle() {
        return toVehicleResponseDtoList(vehicleRepository.findAll());
    }

}
