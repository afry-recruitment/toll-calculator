package com.toll.calculator.service.city;

import com.toll.calculator.dto.city.CityDto;
import com.toll.calculator.dto.city.CityResponseDto;
import com.toll.calculator.model.City;
import com.toll.calculator.repository.CityRepository;
import com.toll.calculator.repository.HolidayRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.HashSet;
import java.util.List;
import java.util.NoSuchElementException;

import static com.toll.calculator.util.CityMapper.toCityResponseDto;
import static com.toll.calculator.util.CityMapper.toCityResponseDtos;


@Service
public class CityServiceImpl implements CityService{
    @Autowired
    CityRepository cityRepository;

    @Autowired
    HolidayRepository holidayRepository;


    @Override
    public CityResponseDto createCity(CityDto cityDto) {
        City city = new City();
        city.setCode(cityDto.getCode());
        city.setName(cityDto.getName());
        city.setHolidays(new HashSet<>(holidayRepository.findAll()));
        return toCityResponseDto(cityRepository.save(city));
    }

    @Override
    public CityResponseDto getCity(long id) {
        return toCityResponseDto(cityRepository.findById(id).orElseThrow(()-> new NoSuchElementException("City could not be found")));
    }

    @Transactional
    @Override
    public CityResponseDto updateCity(long id, CityDto cityDto) {
        City city = cityRepository.findById(id).orElseThrow(()-> new NoSuchElementException("City could not be found"));
        city.setName(cityDto.getName());
        city.setCode(cityDto.getCode());
        return toCityResponseDto(cityRepository.save(city));
    }

    @Override
    public void deleteCity(long id) {
        cityRepository.deleteById(id);
    }

    @Override
    public List<CityResponseDto> getAllCity() {
        return toCityResponseDtos(cityRepository.findAll());
    }
}
