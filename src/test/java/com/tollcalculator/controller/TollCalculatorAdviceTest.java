package com.tollcalculator.controller;

import com.tollcalculator.boObject.TollCalculatorErrorResponse;
import com.tollcalculator.exceptions.InvalidParameterException;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.http.ResponseEntity;

import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertEquals;

@ExtendWith(MockitoExtension.class)
public class TollCalculatorAdviceTest {

    @InjectMocks
    private TollCalculatorAdvice tollCalculatorAdvice;

    @Test
    public void testInvalidParameterException() {
        ResponseEntity<TollCalculatorErrorResponse> tollErrorResponseResponseEntity = tollCalculatorAdvice.handleInvalidParameterException(new InvalidParameterException("Test"));
        assertNotNull(tollErrorResponseResponseEntity);
        assertEquals(400, tollErrorResponseResponseEntity.getStatusCodeValue());
    }


}
