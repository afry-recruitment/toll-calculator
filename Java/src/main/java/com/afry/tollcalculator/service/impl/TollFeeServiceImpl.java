package com.afry.tollcalculator.service.impl;

import java.time.Instant;
import java.time.LocalTime;
import java.time.ZoneId;
import java.time.format.DateTimeFormatter;
import java.util.Comparator;
import java.util.Date;
import java.util.List;
import java.util.Optional;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.afry.tollcalculator.model.TimeAndFee;
import com.afry.tollcalculator.persistance.entity.TollFee;
import com.afry.tollcalculator.persistance.repo.TollFeeRepository;
import com.afry.tollcalculator.service.TollFeeService;

@Service
public class TollFeeServiceImpl implements TollFeeService{
	private static Logger logger = LoggerFactory.getLogger(TollFeeServiceImpl.class);
	
	@Autowired
	TollFeeRepository tollFeeRepo;

	@Override
	public List<TollFee> getTollFee() {
		return tollFeeRepo.findAll();
	}

	@Override
	public void updateTollFee() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public TollFee createTollFee(TimeAndFee timeAndFee) {
		logger.info("Creating a toll fee for start time = {} and end time = {}",timeAndFee.getStartTime(), timeAndFee.getEndTime());
		TollFee tollFeeEntry = new TollFee();
		tollFeeEntry.setStartTime(LocalTime.from(DateTimeFormatter.ISO_LOCAL_TIME.parse(timeAndFee.getStartTime())));
		tollFeeEntry.setEndTime(LocalTime.from(DateTimeFormatter.ISO_LOCAL_TIME.parse(timeAndFee.getEndTime())));
		tollFeeEntry.setFare(timeAndFee.getFare());
		tollFeeEntry.setDescription(timeAndFee.getDescription());
		
		return tollFeeRepo.save(tollFeeEntry);
	}
	
	public Optional<TollFee> getTollFare(Date date) {
		LocalTime tollPasstime = Instant.ofEpochMilli(date.getTime()).atZone(ZoneId.systemDefault()).toLocalTime();
		return tollFeeRepo.findAll().stream().filter(fee -> fee.getStartTime().isBefore(tollPasstime) && fee.getEndTime().isAfter(tollPasstime) )
					.max(Comparator.comparing(TollFee::getFare));
	}
}
