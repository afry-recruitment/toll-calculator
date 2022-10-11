package com.github.ugurcetin.tollfee;

public class Emergency implements Vehicle {
    public String getType() {
        return "Emergency";
    }

    public boolean isFeeFree() {
        return true;
    }
}
