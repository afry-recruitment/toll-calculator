package com.assignment.toll_calculator.controllers;

import com.assignment.toll_calculator.models.response.InfoResponse;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import static com.assignment.toll_calculator.utils.ConstantUtils.INFO_URL;

/**
 * This is the rest controller class where the endpoints giving info about the application.
 */
@RestController
public class InfoController {

    @Value("${spring.application.name}")
    private String applicationName;

    @GetMapping(value = INFO_URL, produces = MediaType.APPLICATION_JSON_VALUE)
    public ResponseEntity<InfoResponse> appInfo() {
        return new ResponseEntity<>(new InfoResponse(applicationName), HttpStatus.OK);
    }

}
