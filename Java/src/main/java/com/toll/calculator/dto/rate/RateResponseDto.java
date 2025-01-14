package com.toll.calculator.dto.rate;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Data;

@Data
public class RateResponseDto {
    private Long id;

    @JsonProperty(CITY_CODE)
    private String cityCode;
    private static final String CITY_CODE = "city_code";

    @JsonProperty(START_TIME)
    private String startTime;
    private static final String START_TIME = "start_time";

    @JsonProperty(END_TIME)
    private String endTime;
    private static final String END_TIME = "end_time";

    private double toll;
}
