package com.afry.test.tollcalculator.config;

import java.time.LocalDate;
import java.time.ZoneId;
import java.util.List;

import jakarta.validation.constraints.Min;

import org.springframework.boot.context.properties.ConfigurationProperties;

import com.afry.test.tollcalculator.domain.TollFeeRule;
import com.afry.test.tollcalculator.domain.VehicleType;

@ConfigurationProperties(prefix = "toll")
public record TollConfig(@Min(0) int dailyMaxFee,
                         ZoneId zoneId,
                         List<LocalDate> exemptedDates,
                         List<TollFeeRule> feeRules,
                         List<VehicleType> tollFreeVehicles,
                         List<LocalDate> tollFreeDates) {

}
