package com.afry.tollcalculator.service.impl;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.time.LocalDate;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.afry.tollcalculator.persistance.entity.TollFee;
import com.afry.tollcalculator.service.FeeCalculatorService;

@Service
public class FeeCalculatorServiceImpl implements FeeCalculatorService {
	
	private final Logger logger = LoggerFactory.getLogger(FeeCalculatorServiceImpl.class);
	
	@Autowired
	TollFreeServiceImpl tollFreeService;
	
	@Autowired
	TollFeeServiceImpl tollFeeService;

	@Override
	public Integer calculateTollFee(String vehicle, List<String> dates) {
		logger.debug("Calculating toll fee for vehicle = {}", vehicle);
		int totalFee = 0;
		
		List<Date> passingTimes = dates.stream().map(dateAsString -> {
			SimpleDateFormat formatter = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss");
			Date date = null;
			try {
				date = formatter.parse(dateAsString);
			} catch (ParseException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			return date;
		}).filter(date -> {
			Calendar calendar = Calendar.getInstance();
			calendar.setTime(date);
			return calendar.get(Calendar.YEAR) == LocalDate.now().getYear();
		}).collect(Collectors.toList());

		
		if(tollFreeService.isTollFreeDate(passingTimes.get(0)) || tollFreeService.isTollFreeVehicle(vehicle))
			return Integer.valueOf(totalFee);
		
		Map<Long, Integer> fareTable = new HashMap<>();
		for(Date date : passingTimes) {
			TollFee tollFee = tollFeeService.getTollFare(date).orElse(null);
			
			if(tollFee != null) {
				if(fareTable.get(tollFee.getId()) == null)
					fareTable.put(tollFee.getId(), tollFee.getFare());
				else if(tollFee.getFare() > fareTable.get(tollFee.getId()))
					fareTable.put(tollFee.getId(), tollFee.getFare());
			}
		}
		
		totalFee = fareTable.values().stream().reduce(0, Integer::sum);
		if(totalFee > 60) totalFee = 60;
		
		return Integer.valueOf(totalFee);
	}
	
}
