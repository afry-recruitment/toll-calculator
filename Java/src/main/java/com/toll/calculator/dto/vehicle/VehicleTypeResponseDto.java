package com.toll.calculator.dto.vehicle;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@Data
@AllArgsConstructor
@NoArgsConstructor
public class VehicleTypeResponseDto {
    private Long id;

    private String name;

    private boolean isTollFree;
}
