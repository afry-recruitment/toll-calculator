package Vehicles;

public class Vehicle {
    private String Registration;

    //overridden in each class of vehicle and returns its type
    public String getType() {
        return "Unspecified";
    }
    //returns its registration
    public String getRegistration() {
        return this.Registration;
    }
    //sets the registration
    void setRegistration(String RegNr) {
        this.Registration = RegNr;
    }

    //returns toll free or not, overriden in classes for toll free vehicles
    public Boolean isTollFree() {
        return false;
    }
}
