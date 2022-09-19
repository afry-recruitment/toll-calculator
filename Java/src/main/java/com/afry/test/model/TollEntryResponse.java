package com.afry.test.model;

/**
 * TollEntryResponse - Use to capture request Data form client response
 */
public class TollEntryResponse {
    private Integer tollFee;
    private String type;
    private String vehicleId;

    public String getVehicleId() {
        return vehicleId;
    }
    public void setVehicleId(String vehicleId) {
        this.vehicleId = vehicleId;
    }

    public Integer getTollFee() {
        return tollFee;
    }
    public void setTollFee(Integer tollFee) {
        this.tollFee = tollFee;
    }

    public String getType() {
        return type;
    }
    public void setType(String type) {
        this.type = type;
    }
}
