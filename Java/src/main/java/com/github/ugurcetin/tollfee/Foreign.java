package com.github.ugurcetin.tollfee;

public class Foreign implements Vehicle {
    public String getType() {
        return "Foreign";
    }

    public boolean isFeeFree() {
        return true;
    }
}
