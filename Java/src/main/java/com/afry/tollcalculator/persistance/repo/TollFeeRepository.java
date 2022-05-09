package com.afry.tollcalculator.persistance.repo;

import org.springframework.data.jpa.repository.JpaRepository;

import com.afry.tollcalculator.persistance.entity.TollFee;

public interface TollFeeRepository extends JpaRepository<TollFee, Long>{

}
