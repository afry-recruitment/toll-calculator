package model.vehicle;

public enum VehicleType {

	Car("Car", false),
    MOTORBIKE("Motorbike", true),
    TRACTOR("Tractor", true),
    EMERGENCY("Emergency", true),
    DIPLOMAT("Diplomat", true),
    FOREIGN("Foreign", true),
    MILITARY("Military", true);
	
	
    private final String type;
    private final boolean tollFree;

    VehicleType(String type, boolean toolFree) {
      this.type = type;
      this.tollFree = toolFree;
    }

    public String getType() {
      return type;
    }

	public boolean isTollFree() {
		return tollFree;
	}
    
    
}
