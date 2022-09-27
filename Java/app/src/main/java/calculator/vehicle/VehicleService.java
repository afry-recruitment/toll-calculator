package calculator.vehicle;

import java.util.Set;

public interface VehicleService
{
     boolean isToolFree(String vehicleType);
     Set<VehicleType> listVehicleTypes();
     public Set<String> listVehicleTypeNames();
     VehicleType getVehicleType(String name);
}
