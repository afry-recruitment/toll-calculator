package com.toll.model;

public class Diplomat implements Vehicle {
    private String number;

    @Override
    public String getType() {
        return "Diplomat";
    }

}
