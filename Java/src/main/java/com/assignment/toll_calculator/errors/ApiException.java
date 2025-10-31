package com.assignment.toll_calculator.errors;

import lombok.Getter;
import org.springframework.http.HttpStatus;

/**
 * This is the exception that we can use customize the output for the user
 * because we can define different status error for the different events.
 */
@Getter
public class ApiException extends RuntimeException {

    private final HttpStatus status;

    public ApiException(Throwable throwable, HttpStatus status) {
        super(throwable.getMessage(), throwable);
        this.status = status;
    }

}
