package com.github.ugurcetin.tollfee;


import java.util.Date;
import java.util.List;

public interface TollFeeCalculator {

    int calculateTollFee(Vehicle vehicle, Date date);

    int calculateTollFee(Vehicle vehicle, List<Date> dates);
}
