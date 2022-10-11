package com.github.ugurcetin.tollfee;

public class Car implements Vehicle {
  public String getType() {
    return "Car";
  }

  public boolean isFeeFree() {
    return false;
  }
}
