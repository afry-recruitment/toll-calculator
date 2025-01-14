package com.toll.calculator.repository;

import com.toll.calculator.model.City;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface CityRepository extends JpaRepository<City, Long> {

	City findOneByCode(String code);
}
