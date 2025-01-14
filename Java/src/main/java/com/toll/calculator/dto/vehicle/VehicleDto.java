package com.toll.calculator.dto.vehicle;

import lombok.AllArgsConstructor;
import lombok.Data;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

@Data
@AllArgsConstructor
public class VehicleDto {
    private String regNumber;

    private long vehicleTypeId;

    private boolean isTollFree;

    private List<Date> tollDates;
}
