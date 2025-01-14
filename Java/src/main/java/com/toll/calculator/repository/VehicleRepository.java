package com.toll.calculator.repository;

import com.toll.calculator.model.vehicle.Vehicle;
import com.toll.calculator.model.vehicle.VehicleEntity;
import org.springframework.data.jpa.repository.JpaRepository;

public interface VehicleRepository extends JpaRepository<VehicleEntity, Long> {
    Vehicle findVehicleByRegNumber(String regNumber);
}
