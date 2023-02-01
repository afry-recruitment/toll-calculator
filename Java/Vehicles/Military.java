package Vehicles;

public class Military extends Vehicle{
    @Override
    public String getType() {
        return "Military";
    }

    @Override
    public Boolean isTollFree() {
        return true;
    }
}
