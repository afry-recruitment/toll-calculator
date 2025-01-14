package com.toll.calculator.service.rate;

import com.toll.calculator.dto.rate.RateDto;
import com.toll.calculator.dto.rate.RateResponseDto;

import java.util.List;

public interface RateService {
    RateResponseDto createRate(RateDto rateDto);
    RateResponseDto getRate(long id);
    RateResponseDto updateRate(long id, RateDto rateDto);
    void deleteRate(long id);
    List<RateResponseDto> getAllRate();
}
