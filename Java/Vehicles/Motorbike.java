package Vehicles;

public class Motorbike extends Vehicle {
    @Override
    public String getType() {
      return "Motorbike";
    }

    @Override
    public Boolean isTollFree() {
        return true;
    }
}
