package com.afry.tollcalculator.service;

import java.util.Date;
import java.util.List;

public interface FeeCalculatorService {
	public Integer calculateTollFee(String vehicle, List<String> dates);
}
