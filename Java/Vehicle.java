import java.util.Objects;

public final class Vehicle {

    private final VehicleType type;

    public Vehicle(VehicleType type) {
        this.type = Objects.requireNonNull(type, "Vehicle type cannot be null");
    }

    public boolean isTollFree() {
        return type.isTollFree();
    }
}
