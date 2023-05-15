package com.tollcalculator.exceptions;

/**
 * This is custom InvalidParameter exception class which extends checked exception
 */
public class InvalidParameterException extends Exception {
    private String error;

    public InvalidParameterException() {
    }
    public InvalidParameterException(String error) {
        super(error);
    }
}
