

public class Car implements Vehicle {
  @Override
  public boolean isTollFreeVechicle()
  {
	  return false;
  }
  @Override
  public String vehicleType()
  {
	  return "car";
  }
}
