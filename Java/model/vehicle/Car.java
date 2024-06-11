package model.vehicle;


/*
 * 
 * There is no current use case for having a subclass of a vehicle but it would probably be so in real case; 
 */
public class Car extends Vehicle {

	public Car(VehicleType vehicleType) {
		super(vehicleType);
	}
	String fuelType;
	String registrationNumber;
	String environmentalClass;
}
