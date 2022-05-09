package com.afry.tollcalculator.service;

import java.util.List;

import org.springframework.http.ResponseEntity;

import com.afry.tollcalculator.model.FreeDay;
import com.afry.tollcalculator.model.VehicleType;
import com.afry.tollcalculator.persistance.entity.TollFreeDay;
import com.afry.tollcalculator.persistance.entity.TollFreeVehicle;

public interface TollFreeService {
	
	public List<TollFreeVehicle> getTollFreeVehicles();
	
	public ResponseEntity<TollFreeVehicle> createTollFreeVehicle(VehicleType vehicle);
	
	public ResponseEntity<TollFreeVehicle> updateTollFreeVehicle(VehicleType vehicle, Long id);
	
	public void removeTollFreeVehicle(Long id);
	
	public List<TollFreeDay> getTollFreeDays();
	
	public TollFreeDay createTollFreeDay(FreeDay freeDay);
}
