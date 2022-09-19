package com.afry.test.dao;

import com.afry.test.model.Vehicle;
import org.springframework.data.jpa.repository.JpaRepository;

/**
 * TollCalculatorRepository to provide DAO access for Vehicle Table
 */
public interface TollCalculatorRepository extends JpaRepository<Vehicle, String> {

}

