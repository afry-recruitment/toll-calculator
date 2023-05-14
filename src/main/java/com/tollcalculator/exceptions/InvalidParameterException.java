package com.tollcalculator.exceptions;

import org.springframework.http.HttpStatus;

public class InvalidParameterException extends Exception {
    private String message;
    private HttpStatus httpStatus;

    public InvalidParameterException(String message) {
        this.message = message;
    }

    public InvalidParameterException(String message, HttpStatus httpStatus) {
        this.message = message;
        this.httpStatus = httpStatus;
    }

    @Override
    public String getMessage() {
        return message;
    }

    public HttpStatus getHttpStatus() {
        return httpStatus;
    }
}
