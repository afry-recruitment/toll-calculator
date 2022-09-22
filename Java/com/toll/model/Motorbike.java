package com.toll.model;

public class Motorbike implements Vehicle {

  private String number;
  @Override
  public String getType() {
    return "Motorbike";
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
