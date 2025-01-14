package com.toll.calculator.controller;

import com.toll.calculator.dto.rate.RateDto;
import com.toll.calculator.dto.rate.RateResponseDto;
import com.toll.calculator.service.rate.RateService;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;


@RestController
@RequestMapping("/api/v1/rate")
@RequiredArgsConstructor
@Slf4j
public class RateController {
    @Autowired
    RateService rateService;

    @PostMapping
    public ResponseEntity<RateResponseDto> createRate(@RequestBody RateDto rateDto){
        return new ResponseEntity<>(rateService.createRate(rateDto), HttpStatus.CREATED);
    }

    @GetMapping
    public ResponseEntity<List<RateResponseDto>> getAllRate(){
        return new ResponseEntity<>(rateService.getAllRate(), HttpStatus.OK);
    }

    @PutMapping("/{id}")
    public ResponseEntity<RateResponseDto> updateRate(@RequestBody RateDto rateDto,@PathVariable(value = "id", required = true) long id){
        return new ResponseEntity<>(rateService.updateRate(id, rateDto), HttpStatus.OK);
    }

    @GetMapping("/{id}")
    public ResponseEntity<RateResponseDto> getRate(@PathVariable(value = "id", required = true) long id){
        return new ResponseEntity<>(rateService.getRate(id), HttpStatus.OK);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteCity(@PathVariable(value = "id", required = true) long id){
        rateService.deleteRate(id);
        return new ResponseEntity<>(HttpStatus.NO_CONTENT);
    }
}
