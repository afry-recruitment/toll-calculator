package com.toll.calculator.dto.vehicle;

import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Data;

@Data
public class VehicleTollRequest {

	@JsonProperty(CITY_CODE)
	public String cityCode;
	public static final String CITY_CODE = "city_code";

	@JsonProperty(REG_NUMBER)
	public String regNumber;
	public static final String REG_NUMBER = "reg_number";

}
