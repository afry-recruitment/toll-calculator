package com.toll.model;

public class Foreign implements Vehicle {
    private String number;

    @Override
    public String getType() {
        return "Foreign";
    }

}
