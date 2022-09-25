package com.toll.model;

public class Car implements Vehicle {

    private String number;

    @Override
    public String getType() {
        return "Car";
    }

}
