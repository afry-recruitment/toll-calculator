import java.util.Objects;

public record Vehicle(VehicleType type) {

    public Vehicle(VehicleType type) {
        this.type = Objects.requireNonNull(type, "Vehicle type cannot be null");
    }

    public boolean isTollFree() {
        return type.isTollFree();
    }
}
