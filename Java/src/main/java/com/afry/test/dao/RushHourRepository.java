package com.afry.test.dao;

import com.afry.test.model.RushHour;
import org.springframework.data.jpa.repository.JpaRepository;

/**
 * RushHourRepository to provide DAO access for RushHour Table
 */
public interface RushHourRepository extends JpaRepository<RushHour, Integer> {
}
