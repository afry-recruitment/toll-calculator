// Tractor is not a car or motorbike. It deserves an own class.

public class Tractor implements Vehicle {
    String type = "Tractor";

    @Override
    public String getType() {
        return type;
    }
}