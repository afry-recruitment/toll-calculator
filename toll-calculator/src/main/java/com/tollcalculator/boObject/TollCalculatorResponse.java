package com.tollcalculator.boObject;

import lombok.Builder;
import lombok.Getter;
import lombok.Setter;

import java.math.BigDecimal;

@Getter
@Setter
@Builder
public class TollCalculatorResponse {

    private BigDecimal tollFeeAmount;
}
