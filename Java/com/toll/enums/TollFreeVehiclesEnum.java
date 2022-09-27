package com.toll.enums;

public enum TollFreeVehiclesEnum {
    MOTORBIKE("Motorbike"), TRACTOR("Tractor"), EMERGENCY("Emergency"), DIPLOMAT("Diplomat"), FOREIGN("Foreign"),
    MILITARY("Military");

    private final String type;

    TollFreeVehiclesEnum(String type) {
        this.type = type;
    }

    public String getType() {
        return type;
    }
}