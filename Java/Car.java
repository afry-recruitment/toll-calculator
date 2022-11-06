
public class Car implements Vehicle {
public String type = "";
@Override
  
  public String getType() { // To provide the type of the car
    return type;
  }
  public void setType(String type) { //to change the type of the car
	 this.type=type; 
	  
  }
}
