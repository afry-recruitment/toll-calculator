

public class Motorbike implements Vehicle {
  @Override
  public boolean isTollFreeVechicle()
  {
	  return true;
  }
  
  @Override
  public String vehicleType()
  {
	  return "Motorbike";
  }
}
