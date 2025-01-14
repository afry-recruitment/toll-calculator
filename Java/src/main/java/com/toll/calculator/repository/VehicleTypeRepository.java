package com.toll.calculator.repository;

import com.toll.calculator.model.vehicle.VehicleType;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

public interface VehicleTypeRepository extends JpaRepository <VehicleType, Long>{
    Optional<VehicleType> findVehicleTypeByName(String code);
}
