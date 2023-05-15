package com.tollcalculator.controller;

import com.tollcalculator.boObject.TollCalculatorErrorResponse;
import com.tollcalculator.exceptions.InvalidParameterException;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.servlet.mvc.method.annotation.ResponseEntityExceptionHandler;
import java.time.LocalDateTime;

@ControllerAdvice
public class TollCalculatorAdvice extends ResponseEntityExceptionHandler {

    Logger LOG = LoggerFactory.getLogger(TollCalculatorAdvice.class);

    @ExceptionHandler(InvalidParameterException.class)
    public final ResponseEntity<TollCalculatorErrorResponse> handleInvalidParameterException(InvalidParameterException ex) {
        LOG.error("Invalid Parameter Exception",ex);
        TollCalculatorErrorResponse tollCalculatorErrorResponse = new TollCalculatorErrorResponse();
        tollCalculatorErrorResponse.setTimestamp(LocalDateTime.now());
        tollCalculatorErrorResponse.setStatus(HttpStatus.BAD_REQUEST);
        tollCalculatorErrorResponse.setMessage(ex.getMessage());
        return new ResponseEntity(tollCalculatorErrorResponse, HttpStatus.BAD_REQUEST);
    }
}

