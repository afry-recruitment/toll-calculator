package com.toll.calculator.util;

import com.toll.calculator.dto.HolidayResponseDto;
import com.toll.calculator.dto.city.CityResponseDto;
import com.toll.calculator.model.City;
import com.toll.calculator.model.Holiday;

import java.text.SimpleDateFormat;
import java.util.List;
import java.util.Set;
import java.util.stream.Collectors;

public class CityMapper {
    private static final SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd");

    private static HolidayResponseDto toHolidayResponseDto(Holiday holiday) {
        if (holiday == null) {
            return null;
        }

        HolidayResponseDto dto = new HolidayResponseDto();
        dto.setId(holiday.getId());
        dto.setCity(holiday.getCity().getCode());
        dto.setDate(dateFormat.format(holiday.getDate()));

        dto.setDate(dateFormat.format(holiday.getDate()));

        return dto;
    }

    public static List<CityResponseDto> toCityResponseDtos(List<City> cities) {
        return cities.stream()
                .map(CityMapper::toCityResponseDto)
                .collect(Collectors.toList());
    }

    public static CityResponseDto toCityResponseDto(City city) {
        if (city == null) {
            return null;
        }

        CityResponseDto dto = new CityResponseDto();
        dto.setId(city.getId());
        dto.setName(city.getName());
        dto.setCode(city.getCode());

        Set<HolidayResponseDto> holidayDtos = city.getHolidays().stream()
                .map(CityMapper::toHolidayResponseDto)
                .collect(Collectors.toSet());
        dto.setHolidays(holidayDtos);

        return dto;
    }
}
