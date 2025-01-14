package com.toll.calculator.dto.rate;

import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
public class RateDto {
    private String cityCode;
    private String startDate;
    private String endDate;
    private double toll;
}
