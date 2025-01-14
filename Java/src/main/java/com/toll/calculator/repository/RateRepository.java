package com.toll.calculator.repository;

import com.toll.calculator.model.City;
import com.toll.calculator.model.Rate;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

@Repository
public interface RateRepository extends JpaRepository<Rate, Long> {
	
	List<Rate> findByCity(City city);

}
