package com.tollcalculator.boObject;

import io.swagger.v3.oas.annotations.media.Schema;
import lombok.Data;
@Data
public class VehicleType {
    @Schema(example = "Car")
    private String type;
}
