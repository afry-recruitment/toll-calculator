package com.assignment.toll_calculator.models;

import lombok.AllArgsConstructor;
import lombok.Getter;

/**
 * This is the model for an API call successful toll fee response
 */
@AllArgsConstructor
@Getter
public class TollResponse {

    private int fee;

}
