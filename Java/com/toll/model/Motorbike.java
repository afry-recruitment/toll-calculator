package com.toll.model;

public class Motorbike implements Vehicle {

    private String number;

    @Override
    public String getType() {
        return "Motorbike";
    }

}
