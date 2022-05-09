package com.afry.tollcalculator.rest;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.afry.tollcalculator.model.CalculateFee;
import com.afry.tollcalculator.model.FreeDay;
import com.afry.tollcalculator.model.TimeAndFee;
import com.afry.tollcalculator.model.VehicleType;
import com.afry.tollcalculator.persistance.entity.TollFee;
import com.afry.tollcalculator.persistance.entity.TollFreeDay;
import com.afry.tollcalculator.persistance.entity.TollFreeVehicle;
import com.afry.tollcalculator.service.FeeCalculatorService;
import com.afry.tollcalculator.service.TollFeeService;
import com.afry.tollcalculator.service.TollFreeService;

@RestController
@RequestMapping("/api/v1")
public class TollFeeController {
	
	@Autowired
	private TollFreeService tollFreeService;
	@Autowired
	private TollFeeService tollFeeService;
	@Autowired
	private FeeCalculatorService feeCalculatorService;
	

	@GetMapping("/tollfree/vehicle/all")
    public ResponseEntity<List<TollFreeVehicle>> getAllTollFreeVehicles(){
        List<TollFreeVehicle> tollFreeVehicles = tollFreeService.getTollFreeVehicles();
        return ResponseEntity.ok(tollFreeVehicles);
    } 
	
	@PostMapping("/tollfree/vehicle")
	public ResponseEntity<TollFreeVehicle> createTollFreeVehicle(@RequestBody VehicleType vehicle){
		return tollFreeService.createTollFreeVehicle(vehicle);
	}
	
	@PutMapping("/tollfree/vehicle/{id}")
	public ResponseEntity<TollFreeVehicle> updateTollFreeVehicle(@RequestBody VehicleType vehicle, @PathVariable("id") Long id){
		return tollFreeService.updateTollFreeVehicle(vehicle, id);
	}
	
	@GetMapping("/tollfree/day/all")
    public ResponseEntity<List<TollFreeDay>> getAllTollFreeDays(){
        List<TollFreeDay> tollFreeDays = tollFreeService.getTollFreeDays();
        return ResponseEntity.ok(tollFreeDays);
    }
	
	@PostMapping("/tollfree/day")
	public ResponseEntity<TollFreeDay> createTollFreeDay(@RequestBody FreeDay freeDay){
		TollFreeDay tollFreeDay = tollFreeService.createTollFreeDay(freeDay);
		return new ResponseEntity<TollFreeDay>(tollFreeDay, HttpStatus.CREATED);
	}
	
	@GetMapping("/tollfee/all")
    public ResponseEntity<List<TollFee>> getTollFee(){
        List<TollFee> tollFee = tollFeeService.getTollFee();
        return ResponseEntity.ok(tollFee);
    }
	
	@PostMapping("/tollfee")
	public ResponseEntity<TollFee> createTollFee(@RequestBody TimeAndFee timeAndFee){
		TollFee tollFee = tollFeeService.createTollFee(timeAndFee);
		return new ResponseEntity<TollFee>(tollFee, HttpStatus.CREATED);
	}
	
	@PostMapping("/tollfee/calculate")
	public ResponseEntity<Integer> calculateTollFee(@RequestBody CalculateFee feeData){
		Integer totalFee = feeCalculatorService.calculateTollFee(feeData.getVehicleType(), feeData.getPassingTimes());
		return new ResponseEntity<Integer>(totalFee, HttpStatus.OK);
	}
}
