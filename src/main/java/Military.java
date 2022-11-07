public class Military implements Vehicle {
    @Override
    public VehicleType getType() {
        return VehicleType.MILITARY;
    }

    @Override
    public boolean isTollFree() {
        return true;
    }
}
