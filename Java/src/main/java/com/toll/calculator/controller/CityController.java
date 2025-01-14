package com.toll.calculator.controller;

import com.toll.calculator.dto.city.CityDto;
import com.toll.calculator.dto.city.CityResponseDto;
import com.toll.calculator.service.city.CityService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/v1/city")
@RequiredArgsConstructor
@Slf4j
public class CityController {
    @Autowired
    CityService cityService;

    @PostMapping
    public ResponseEntity<CityResponseDto> createCity(@RequestBody CityDto cityDto) {
        return new ResponseEntity<>(cityService.createCity(cityDto), HttpStatus.CREATED);
    }

    @GetMapping
    public ResponseEntity<List<CityResponseDto>> getAllCity() {
        return new ResponseEntity<>(cityService.getAllCity(), HttpStatus.OK);
    }

    @PutMapping("/{id}")
    public ResponseEntity<CityResponseDto> updateCity(@RequestBody CityDto cityDto, @PathVariable(value = "id", required = true) long id) {
        return new ResponseEntity<>(cityService.updateCity(id, cityDto), HttpStatus.OK);
    }

    @GetMapping("/{id}")
    public ResponseEntity<CityResponseDto> getCity(@PathVariable(value = "id", required = true) long id) {
        return new ResponseEntity<>(cityService.getCity(id), HttpStatus.OK);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteCity(@PathVariable(value = "id", required = true) long id) {
        cityService.deleteCity(id);
        return new ResponseEntity<>(HttpStatus.NO_CONTENT);
    }
}
