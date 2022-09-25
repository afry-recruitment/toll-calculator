package com.toll.model;

public class Military implements Vehicle {

    private String number;

    @Override
    public String getType() {
        return "Military";
    }

}
