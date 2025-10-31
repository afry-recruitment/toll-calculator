package com.assignment.toll_calculator.models;

import lombok.*;
import org.springframework.http.HttpStatus;

/**
 * This is the model for an API call error response
 */
@AllArgsConstructor
@Builder
@Getter
@Setter
public class ErrorResponse {

    private String timestamp;
    private HttpStatus status;
    private Integer statusCode;
    private String type;
    private String message;
    private String cause;

}
