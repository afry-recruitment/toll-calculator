package com.toll.calculator.dto;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Data;


@Data
public class HolidayResponseDto {
    private Long id;
    @JsonProperty(CITY_CODE)
    private String city;
    private static final String CITY_CODE = "city_code";
    private String date;

}
