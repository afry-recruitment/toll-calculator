package com.toll.model;

public class Car implements Vehicle {

private String number;
  @Override
  public String getType() {
    return "Car";
  }

  @Override
  public String getNumber() {
    return number;
  }

  @Override
  public void setNumber(String number) {
    this.number = number;
  }
}
