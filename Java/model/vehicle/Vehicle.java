package model.vehicle;

public class Vehicle {

	private VehicleType vehicleType;

	
	public Vehicle(VehicleType vehicleType) {
		super();
		this.vehicleType = vehicleType;
	}

	public VehicleType getVehicleType() {
		return vehicleType;
	}

	public void setVehicleType(VehicleType vehicleType) {
		this.vehicleType = vehicleType;
	}

}
