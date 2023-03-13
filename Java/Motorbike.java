// Created an attribute which is being returned when calling the method
public class Motorbike implements Vehicle {
  String type = "Motorbike";

  @Override
  public String getType() {
    return type;
  }
}
