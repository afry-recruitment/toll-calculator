package Vehicles;

public class Emergency extends Vehicle{
    @Override
    public String getType() {
        return "Emergency";
    }

    @Override
    public Boolean isTollFree() {
        return true;
    }
}
