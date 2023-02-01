package Vehicles;

public class Foreign extends Vehicle{
    @Override
    public String getType() {
        return "Foreign";
    }

    @Override
    public Boolean isTollFree() {
        return true;
    }
}
