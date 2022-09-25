package com.toll.model;

public class Emergency implements Vehicle {
    private String number;

    @Override
    public String getType() {
        return "Emergency";
    }

}
