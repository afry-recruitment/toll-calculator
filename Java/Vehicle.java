public class Vehicle {
    
    public enum VehicleList {
        MOTORBIKE,
        TRACTOR,
        EMERGENCY,
        DIPLOMAT,
        FOREIGN,
        MILITARY,
        OTHERS;
    }

    public boolean isVehicleTollFree(VehicleList vehicle) {
        if (vehicle == VehicleList.OTHERS) {
            return false;
        } else {
            return true;
        }
    }
}
