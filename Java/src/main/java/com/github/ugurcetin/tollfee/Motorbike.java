package com.github.ugurcetin.tollfee;

public class Motorbike implements Vehicle {
  public String getType() {
    return "Motorbike";
  }

  public boolean isFeeFree() {
    return true;
  }
}
