package com.toll.calculator.dto.vehicle;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.toll.calculator.model.vehicle.VehicleType;
import lombok.Data;

import java.util.ArrayList;
import java.util.List;

@Data
public class VehicleResponseDto {
    @JsonProperty(VEHICLE_ID)
    private Long vehicleId;
    private static final String VEHICLE_ID = "vehicle_id";

    @JsonProperty(REG_NUM)
    private String regNumber;
    private static final String REG_NUM = "reg_number";

    private VehicleType vehicleType;

    @JsonProperty(TOLL_DATES)
    private List<String> tollDates = new ArrayList<>();
    private static final String TOLL_DATES = "toll_dates";
}
