package com.toll.calculator.dto.city;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.toll.calculator.dto.HolidayResponseDto;
import lombok.Data;

import java.util.Set;

@Data
public class CityResponseDto {
    private long id;
    private String name;
    @JsonProperty(CITYCODE)
    private String code;
    private static final String CITYCODE = "city_code";
    private Set<HolidayResponseDto> holidays;
}
