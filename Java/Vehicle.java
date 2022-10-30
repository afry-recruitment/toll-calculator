public class Vehicle {

  public static final String tollFreeVehicles = "motorbike, tractor, emergency, diplomat, foreign, military";
  public static String type;
  
  public Vehicle(String string) {
	type = string;
}

static boolean isTollFreeVehicle(Vehicle vehicle) {
	    if(vehicle == null) return false;
	    return Vehicle.type.toLowerCase().contains(tollFreeVehicles);
	  }
}