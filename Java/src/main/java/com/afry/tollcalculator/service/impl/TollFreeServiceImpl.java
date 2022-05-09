package com.afry.tollcalculator.service.impl;

import java.sql.Date;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import com.afry.tollcalculator.model.FreeDay;
import com.afry.tollcalculator.model.VehicleType;
import com.afry.tollcalculator.persistance.entity.TollFreeDay;
import com.afry.tollcalculator.persistance.entity.TollFreeVehicle;
import com.afry.tollcalculator.persistance.repo.TollFreeDayRepository;
import com.afry.tollcalculator.persistance.repo.TollFreeVehicleRepository;
import com.afry.tollcalculator.service.TollFreeService;

@Service
public class TollFreeServiceImpl implements TollFreeService{
	
	private static Logger logger = LoggerFactory.getLogger(TollFreeServiceImpl.class);
	
	@Autowired
	TollFreeVehicleRepository tollFreeVehicleRepo;
	
	@Autowired
	TollFreeDayRepository tollFreeDayRepo;

	@Override
	public List<TollFreeVehicle> getTollFreeVehicles() {
		
		return tollFreeVehicleRepo.findAll();
	}

	@Override
	public ResponseEntity<TollFreeVehicle> createTollFreeVehicle(VehicleType vehicle) {
		logger.info("Creating a toll free vehicle = {}",vehicle.getVehicleType());
		TollFreeVehicle newTollFreeVehicle = new TollFreeVehicle();
		newTollFreeVehicle.setName(vehicle.getVehicleType().toUpperCase());
		newTollFreeVehicle.setDescription(vehicle.getDescription());
		
		tollFreeVehicleRepo.save(newTollFreeVehicle);
		
		return new ResponseEntity<TollFreeVehicle>(newTollFreeVehicle, HttpStatus.CREATED);
	}

	@Override
	public ResponseEntity<TollFreeVehicle> updateTollFreeVehicle(VehicleType vehicle, Long id) {
		logger.info("Updating a toll free vehicle = {}",vehicle.getVehicleType());
		TollFreeVehicle updateVehicle = tollFreeVehicleRepo.getById(id);
		updateVehicle.setName(vehicle.getVehicleType().toUpperCase());
		updateVehicle.setDescription(vehicle.getDescription());
		
		tollFreeVehicleRepo.save(updateVehicle);
		
		return new ResponseEntity<TollFreeVehicle>(updateVehicle, HttpStatus.CREATED);
	}

	@Override
	public void removeTollFreeVehicle(Long id) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public List<TollFreeDay> getTollFreeDays() {
		return tollFreeDayRepo.findAll();
	}

	@Override
	public TollFreeDay createTollFreeDay(FreeDay freeDay) {
		logger.info("Creating a toll free day = {}",freeDay.getDay());
		TollFreeDay tollFreeDay = new TollFreeDay();
		
		Date day = Date.valueOf(freeDay.getDay());  
		tollFreeDay.setDay(day);
		tollFreeDay.setDescription(freeDay.getDescription());
		
		return tollFreeDayRepo.save(tollFreeDay);
	}
	
	public boolean isTollFreeVehicle(String vehicleType) {
		boolean isTollFree = tollFreeVehicleRepo.findByName(vehicleType.toUpperCase()).size() == 0 ? false : true;
		logger.info("Is toll free vehicle = {}",isTollFree);
		return isTollFree; 
	}
	
	public boolean isTollFreeDate(java.util.Date date) {
		boolean isTollFree = tollFreeDayRepo.findByDay(new java.sql.Date(date.getTime())).size() == 0 ? false : true; 
		logger.info("Is toll free day = {}",isTollFree);
		return isTollFree;
	}
}
