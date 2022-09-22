package com.toll.model;

public class Military implements Vehicle {

  private String number;
  @Override
  public String getType() {
    return "Military";
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
