// Created a constructor with a parameter to create different kind of cars. E.g. military, diplomat, foreign etc.
public class Car implements Vehicle {
  String type;

  Car(String t) {
    type = t;
  }

  @Override
  public String getType() {
    return type;
  }
}
