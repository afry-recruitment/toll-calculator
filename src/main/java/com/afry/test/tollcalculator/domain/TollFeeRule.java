package com.afry.test.tollcalculator.domain;

import java.time.LocalTime;

public record TollFeeRule(LocalTime from, LocalTime to, int fee) {}
