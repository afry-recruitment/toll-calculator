package com.tollcalculator.boObject;

import com.tollcalculator.util.DateTimeUtil;
import io.swagger.v3.oas.annotations.media.Schema;

import java.text.ParseException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class TollCalculatorRequest {

    @Schema(example = "Gothenburg")
    private String city;
    private VehicleType vehicle;

    @Schema(example = "[\n" +
            "        \"2013-01-14 06:00:00\",\n" +
            "        \"2013-01-14 06:15:00\"\n" +
            "    ]")
    private List<Date> checkInTime;

    public String getCity() {
        return city;
    }

    public void setCity(String city) {
        this.city = city;
    }

    public VehicleType getVehicle() {
        return vehicle;
    }

    public void setVehicle(VehicleType vehicle) {
        this.vehicle = vehicle;
    }

    public List<Date> getCheckInTime() {
        return checkInTime;
    }

    public void setCheckInTime(List checkInTime) {
        List<Date> dates = new ArrayList<>();
        if (checkInTime != null) {
            checkInTime.forEach(time -> {
                try {
                    Date dateTime = DateTimeUtil.formatToDate(time);
                    dates.add(dateTime);
                } catch (ParseException e) {
                    e.printStackTrace();
                }
            });
        }
        this.checkInTime = dates;
    }
}
