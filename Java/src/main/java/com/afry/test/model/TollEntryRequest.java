package com.afry.test.model;

/**
 * TollEntryRequest - Use to capture request Data form client request
 */
public class TollEntryRequest {
    private String vehicleId;
    private String type;
    public String getVehicleId() {
        return vehicleId;
    }
    public void setVehicleId(String vehicleId) {
        this.vehicleId = vehicleId;
    }
    public String getType() {
        return type;
    }
    public void setType(String type) {
        this.type = type;
    }
}
