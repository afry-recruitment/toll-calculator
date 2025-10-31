package com.assignment.toll_calculator.models;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;

/**
 * This is the model for an API call successful toll fee response
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
public class TollResponse {

    private int fee;

}
