package com.afry.test.model;

import javax.persistence.Entity;

/**
 * Extended Vehicle type -Car
 */
@Entity
public class Car extends Vehicle {
    @Override
    public String getType() {
        return "Car";
    }
}


