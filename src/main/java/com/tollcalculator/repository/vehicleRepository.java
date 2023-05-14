package com.tollcalculator.repository;

import com.tollcalculator.model.Vehicle;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface vehicleRepository extends JpaRepository<Vehicle,Long> {
    Optional<Vehicle> findVehicleByName(String name);
}
