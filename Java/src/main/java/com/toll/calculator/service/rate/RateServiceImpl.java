package com.toll.calculator.service.rate;

import com.toll.calculator.dto.rate.RateDto;
import com.toll.calculator.dto.rate.RateResponseDto;
import com.toll.calculator.exception.ResourceNotFoundException;
import com.toll.calculator.exception.TollException;
import com.toll.calculator.model.City;
import com.toll.calculator.model.Rate;
import com.toll.calculator.repository.CityRepository;
import com.toll.calculator.repository.RateRepository;
import jakarta.transaction.Transactional;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

import static com.toll.calculator.util.DateUtil.getDateFromString;
import static com.toll.calculator.util.RateMapper.toRateResponseDto;
import static com.toll.calculator.util.RateMapper.toRateResponseDtoList;

@Service
@Slf4j
public class RateServiceImpl implements RateService {
    @Autowired
    RateRepository rateRepository;

    @Autowired
    CityRepository cityRepository;

    @Override
    public RateResponseDto createRate(RateDto rateDto) {
        Rate rate = new Rate();

        try {
            City city = cityRepository.findOneByCode(rateDto.getCityCode());
            if (city == null) {
                throw new RuntimeException("City could not be found");
            }
            rate.setCity(city);
            rate.setStartDate(getDateFromString(rateDto.getStartDate()));
            rate.setEndDate(getDateFromString(rateDto.getEndDate()));
            rate.setTax(rateDto.getToll());
        } catch (TollException e) {
            log.error(e.getMessage(), e);
        }
        return toRateResponseDto(rateRepository.save(rate));
    }

    @Override
    public RateResponseDto getRate(long id) {
        return toRateResponseDto(rateRepository.getReferenceById(id));
    }


    @Transactional
    @Override
    public RateResponseDto updateRate(long id, RateDto rateDto) {
        Rate updatedRate = null;
        try {
            Rate rate = rateRepository.findById(id).orElseThrow(() -> new ResourceNotFoundException("Rate could not be found"));
            City city = cityRepository.findOneByCode(rateDto.getCityCode());
            if (city == null) {
                throw new RuntimeException("City could not be found");
            }

            rate.setCity(city);
            rate.setStartDate(getDateFromString(rateDto.getStartDate()));
            rate.setEndDate(getDateFromString(rateDto.getEndDate()));
            rate.setTax(rateDto.getToll());
            updatedRate = rateRepository.save(rate);

        } catch (TollException e) {
            log.error(e.getMessage(), e);
        }
        return toRateResponseDto(updatedRate);
    }

    @Override
    public void deleteRate(long id) {
        rateRepository.deleteById(id);
    }

    @Override
    public List<RateResponseDto> getAllRate() {
        return toRateResponseDtoList(rateRepository.findAll());
    }
}
