package com.afry.tollcalculator.persistance.repo;

import java.sql.Date;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;

import com.afry.tollcalculator.persistance.entity.TollFreeDay;

public interface TollFreeDayRepository extends JpaRepository<TollFreeDay, Long>{
	
	public List<TollFreeDay> findByDay(Date date);
}
