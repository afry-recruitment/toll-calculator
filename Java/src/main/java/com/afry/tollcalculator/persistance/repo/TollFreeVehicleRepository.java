package com.afry.tollcalculator.persistance.repo;

import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.afry.tollcalculator.persistance.entity.TollFreeVehicle;

public interface TollFreeVehicleRepository extends JpaRepository<TollFreeVehicle, Long>{
	
	List<TollFreeVehicle> findByName(String name);
	
}
