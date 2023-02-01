package Vehicles;

public class Diplomat extends Vehicle{
    @Override
    public String getType() {
        return "Diplomat";
    }

    @Override
    public Boolean isTollFree() {
        return true;
    }
}
