package Vehicles;

import Exception.CarCreatorException;

public class VehicleCreator {
    //create vehicles
    public Vehicle createVehicle(String Type, String registration) throws CarCreatorException {
        Vehicle vehicle = null;
        if (Type == null)
            return null;

        switch(Type) {
            case "Car":
                vehicle = new Car();
                vehicle.setRegistration(registration);
                return vehicle;
            case "Diplomat":
                vehicle = new Diplomat();
                vehicle.setRegistration(registration);
                return vehicle;
            case "Emergency":
                vehicle = new Emergency();
                vehicle.setRegistration(registration);
                return vehicle;
            case "Foreign":
                vehicle = new Foreign();
                vehicle.setRegistration(registration);
                return vehicle;
            case "Military":
                vehicle = new Military();
                vehicle.setRegistration(registration);
                return vehicle;
            case "Motorbike":
                vehicle = new Motorbike();
                vehicle.setRegistration(registration);
                return vehicle;
            case "Tractor":
                vehicle = new Tractor();
                vehicle.setRegistration(registration);
                return vehicle;
            default:
                throw new CarCreatorException(Type + "\" is not a valid vehicle!");
        }    
    }
}
