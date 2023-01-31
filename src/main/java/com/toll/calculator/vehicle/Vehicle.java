package com.toll.calculator.vehicle;

public interface Vehicle {

    //https://www.transportstyrelsen.se/sv/vagtrafik/Trangselskatt/Undantag-fran-trangselskatt/
    enum VehicleType {
        PRIVATE_CAR("Private Car",false),
        DIPLOMAT_PRIVATE_CAR("Diplomat Private Car", true),
        EMERGENCY("Emergency", true),
        BUS("Bus", true),
        MINIBUS("Minibus", false),
        MOTORCYCLE("Motorcycle", true),
        MILITARY("Military", true),
        TRACTOR("Tractor", true),
        TRUCK("Truck", false);

        private final String name;
        private final boolean tollFree;
        VehicleType(String name, boolean tollFree) {
            this.name = name;
            this.tollFree = tollFree;
        }

        public String getName() {
            return name;
        }

        public boolean isTollFree() {
            return tollFree;
        }
    }
    VehicleType getType();

    String getRegNumber();

    void setRegNumber(String regNumber);
}
