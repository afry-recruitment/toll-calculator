package com.tollcalculator.Exception;

import com.tollcalculator.exceptions.InvalidParameterException;
import org.junit.jupiter.api.Test;

public class InvalidParameterExceptionTest {
    @Test
    public void testConstructorNoArgs() {
        InvalidParameterException invalidParameterException = new InvalidParameterException();
    }

    @Test
    public void testConstructorWithArgs() {
        InvalidParameterException invalidParameterExceptionWithArgs = new InvalidParameterException("Invalid Parameter Exception");
    }
}
