package com.github.ugurcetin.tollfee;

public class Tractor implements Vehicle {
    public String getType() {
        return "Tractor";
    }

    public boolean isFeeFree() {
        return true;
    }
}
