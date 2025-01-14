package com.toll.calculator.dto;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Data;

@Data
public class TollResponse {

	@JsonProperty(ATTR_TAX)
	public double toll;
	public static final String ATTR_TAX = "toll";

	@JsonProperty(ATTR_CURRENCY)
	public String currency = "SEK";
	public static final String ATTR_CURRENCY = "currency";

}
