package com.afry.tollcalculator;

import java.util.Optional;

enum TollFreeVehicles {
    CAR("Car", false),
    MOTORBIKE("Motorbike", true),
    TRACTOR("Tractor", true),
    EMERGENCY("Emergency", true),
    DIPLOMAT("Diplomat", true),
    FOREIGN("Foreign", true),
    MILITARY("Military", true);

    private final String type;
    private final boolean tollFree;

    TollFreeVehicles(String type, boolean tollFree) {
        this.type = type;
        this.tollFree = tollFree;
    }

    public static Optional<TollFreeVehicles> fromString(String type) {
        for (TollFreeVehicles vehicle : TollFreeVehicles.values()) {
            if (vehicle.getType().equalsIgnoreCase(type)) {
                return Optional.of(vehicle);
            }
        }
        return Optional.empty();
    }

    String getType() {
        return type;
    }

    boolean isTollFree() {
        return tollFree;
    }
}
