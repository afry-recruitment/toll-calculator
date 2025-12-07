package afry.tollcollector.domain;

import java.util.List;

public record TollFreeVehicleTypes(List<String> tollFreeVehicleTypes) {

  public boolean isTollFreeVehicleType(Vehicle vehicle) {
    return tollFreeVehicleTypes.contains(vehicle.getType());
  }
}
