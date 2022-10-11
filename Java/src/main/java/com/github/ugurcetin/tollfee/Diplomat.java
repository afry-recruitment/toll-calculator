package com.github.ugurcetin.tollfee;

public class Diplomat implements Vehicle {
    public String getType() {
        return "Diplomat";
    }

    public boolean isFeeFree() {
        return true;
    }

}
