package model;

public enum TollFreeVehicles {

        MOTORBIKE("model.Motorbike"),
        TRACTOR("Tractor"),
        EMERGENCY("Emergency"),
        DIPLOMAT("Diplomat"),
        FOREIGN("Foreign"),
        MILITARY("Military");

        private final String type;

        TollFreeVehicles(String type) {
            this.type = type;
        }

        public String getType() {
            return type;
        }
    }

