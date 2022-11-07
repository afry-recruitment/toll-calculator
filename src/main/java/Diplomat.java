public class Diplomat implements Vehicle {
    @Override
    public VehicleType getType() {
        return VehicleType.DIPLOMAT;
    }

    @Override
    public boolean isTollFree() {
        return true;
    }
}
