package Vehicles;

public class Tractor extends Vehicle {
    @Override
    public String getType() {
        return "Tractor";
    }

    @Override
    public Boolean isTollFree() {
        return true;
    }
}
