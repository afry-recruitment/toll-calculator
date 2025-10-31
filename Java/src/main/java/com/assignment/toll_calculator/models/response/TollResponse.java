package com.assignment.toll_calculator.models.response;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.Setter;

/**
 * This is the model for an API call successful toll fee response
 */
@AllArgsConstructor
@Getter
@Setter
public class TollResponse {

    private Integer fee;

}
