package model.toll.event;
import java.time.LocalDateTime;
import java.time.LocalTime;
import java.util.Date;

import model.toll.TollFee;
import model.vehicle.Vehicle;


public class TollEvent{

	private Vehicle vehicle;
	
	private LocalTime date;
	
	private TollEvenType type;
	
	private TollFee tollFee;
	
	private LocalDateTime intervalStart;
	

	public TollEvent(Vehicle vehicle, LocalTime date, TollEvenType type,LocalDateTime intervalStart ) {
		super();
		this.vehicle = vehicle;
		this.date = date;
		this.type = type;
		this.intervalStart = intervalStart;
	}
	
	public TollFee getTollFee() {
		return tollFee;
	}

	public void setTollFee(TollFee tollFee) {
		this.tollFee = tollFee;
	}

	public Vehicle getVehicle() {
		return vehicle;
	}

	public void setVehicle(Vehicle vehicle) {
		this.vehicle = vehicle;
	}

	public TollEvenType getType() {
		return type;
	}

	public void setType(TollEvenType type) {
		this.type = type;
	}

	public LocalDateTime getIntervalStart() {
		return intervalStart;
	}

	public void setIntervalStart(LocalDateTime intervalStart) {
		this.intervalStart = intervalStart;
	}

	public LocalTime getDate() {
		return date;
	}

	public void setDate(LocalTime date) {
		this.date = date;
	} 
	
	
}
