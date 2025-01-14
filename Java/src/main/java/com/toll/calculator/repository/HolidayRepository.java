package com.toll.calculator.repository;

import com.toll.calculator.model.City;
import com.toll.calculator.model.Holiday;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface HolidayRepository extends JpaRepository<Holiday, Long> {
	
	List<Holiday> findByCity(City city);

}
