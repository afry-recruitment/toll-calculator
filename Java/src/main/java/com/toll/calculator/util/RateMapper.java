package com.toll.calculator.util;

import com.toll.calculator.dto.rate.RateResponseDto;
import com.toll.calculator.model.Rate;

import java.text.SimpleDateFormat;
import java.util.List;
import java.util.stream.Collectors;

public class RateMapper {
    private static final SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

    public static RateResponseDto toRateResponseDto(Rate rate) {
        if (rate == null) {
            return null;
        }

        RateResponseDto dto = new RateResponseDto();
        dto.setId(rate.getId());
        dto.setCityCode(rate.getCity().getCode());
        dto.setStartTime(dateFormat.format(rate.getStartDate()));
        dto.setEndTime(dateFormat.format(rate.getEndDate()));
        dto.setToll(rate.getTax());

        return dto;
    }

    public static List<RateResponseDto> toRateResponseDtoList(List<Rate> rates) {
        return rates.stream()
                .map(RateMapper::toRateResponseDto)
                .collect(Collectors.toList());
    }
}
