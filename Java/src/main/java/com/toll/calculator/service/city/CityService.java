package com.toll.calculator.service.city;


import com.toll.calculator.dto.city.CityDto;
import com.toll.calculator.dto.city.CityResponseDto;

import java.util.List;

public interface CityService {
    CityResponseDto createCity(CityDto cityDto);
    CityResponseDto getCity(long id);
    CityResponseDto updateCity(long id, CityDto cityDto);
    void deleteCity(long id);
    List<CityResponseDto> getAllCity();
}
