package com.afry.tollcalculator.model;

import java.util.Arrays;
import java.util.List;

public class CalculateFee {
	private String vehicleType;
	private List<String> passingTimes;
	
	public CalculateFee() {
		
	}
	
	public CalculateFee(String vehicleType, String... passingTimes) {
		this.vehicleType = vehicleType;
		this.passingTimes = Arrays.asList(passingTimes);
	}
	public String getVehicleType() {
		return vehicleType;
	}
	public void setVehicleType(String vehicleType) {
		this.vehicleType = vehicleType;
	}
	public List<String> getPassingTimes() {
		return passingTimes;
	}
	public void setPassingTimes(List<String> passingTimes) {
		this.passingTimes = passingTimes;
	}
}
