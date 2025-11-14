// This enum represents different types of vehicles and indicates whether they are toll-free.
public enum VehicleType {
    MOTORBIKE(true),
    TRACTOR(false),
    EMERGENCY(true),
    DIPLOMAT(true),
    FOREIGN(true),
    MILITARY(true),
    CAR(false),
    TRUCK(false),
    BUS(false);

    private final boolean tollFree;

    VehicleType(boolean tollFree) {
        this.tollFree = tollFree;
    }

    public boolean isTollFree() {
        return tollFree;
    }
}
