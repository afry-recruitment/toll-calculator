package com.assignment.toll_calculator.errors;

import com.assignment.toll_calculator.models.response.ErrorResponse;
import jakarta.validation.ConstraintViolationException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.MissingServletRequestParameterException;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

import java.time.Instant;

/**
 * This class is for handling the global exceptions and convert them to error response
 */
@RestControllerAdvice
public class GlobalExceptionHandler {

    @ExceptionHandler(value = Exception.class)
    public ResponseEntity<ErrorResponse> handleException(Exception exception) {
        return composeResponse(exception, HttpStatus.INTERNAL_SERVER_ERROR);
    }

    @ExceptionHandler(value = ApiException.class)
    public ResponseEntity<ErrorResponse> handleApiException(ApiException exception) {
        return composeResponse(exception, exception.getStatus());
    }

    @ExceptionHandler(value = {MissingServletRequestParameterException.class, ConstraintViolationException.class})
    public ResponseEntity<ErrorResponse> handleBadRequestException(Exception exception) {
        return composeResponse(exception, HttpStatus.BAD_REQUEST);
    }

    private ResponseEntity<ErrorResponse> composeResponse(Throwable throwable, HttpStatus status) {
        Throwable cause = throwable.getCause();

        return new ResponseEntity<>(
                ErrorResponse.builder()
                        .timestamp(Instant.now().toString())
                        .status(status)
                        .statusCode(status.value())
                        .type(throwable.getClass().getName())
                        .message(throwable.getMessage())
                        .cause(cause != null ? cause.getMessage() : null)
                        .build(),
                status
        );
    }

}
