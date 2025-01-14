package com.toll.calculator.dto;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@Data
@NoArgsConstructor
@AllArgsConstructor
public class TollRequestWithVehicleType {
    private Long vehicleTypeId;
    private String vehicleTypeName;
    private String cityCode;
    private List<String> tollDates;
}
