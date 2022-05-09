package com.afry.tollcalculator.service;

import java.util.List;

import com.afry.tollcalculator.model.TimeAndFee;
import com.afry.tollcalculator.persistance.entity.TollFee;

public interface TollFeeService {
	
	public List<TollFee> getTollFee();
	public TollFee createTollFee(TimeAndFee timeAndFee);
	public void updateTollFee();
}
