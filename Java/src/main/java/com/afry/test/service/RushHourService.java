package com.afry.test.service;

import java.util.*;

import com.afry.test.dao.RushHourRepository;
import com.afry.test.model.RushHour;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

/**
 * RushHourService - Provide Rush Hour Data access
 */
@Service
public class RushHourService {
    @Autowired
    RushHourRepository rushHourRepository;

    /***
     * To GetRushHours form RushHourRepository
     * @return  RushHours
     */
    public List<RushHour> getRushHours() {
        return rushHourRepository.findAll();
    }
}

