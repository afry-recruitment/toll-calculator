package com.github.ugurcetin.tollfee;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.text.ParseException;
import java.text.SimpleDateFormat;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

public class Main {
    public static void main(String[] args) throws ParseException {
        try {
            byte[] holidaysData = Files.readAllBytes(Paths.get("src/main/resources/data/holidays.json"));
            ObjectMapper objectMapper = new ObjectMapper();
            List<Holiday> holidayList = objectMapper.readValue(holidaysData, new TypeReference<ArrayList<Holiday>>() {});
           
            byte[] periodData = Files.readAllBytes(Paths.get("src/main/resources/data/period_fees.json"));
            List<TollFeePeriod> periodList = objectMapper.readValue(periodData, new TypeReference<ArrayList<TollFeePeriod>>() {});
            

            SimpleTollFeeCalculator calculator = new SimpleTollFeeCalculator(periodList, holidayList);
            SimpleDateFormat sdf = new SimpleDateFormat("HH:mm:ss");
            Date firstDate = sdf.parse("06:23:00");
            Date secondDate = sdf.parse("15:56:00");
            
            int fee = calculator.calculateTollFee(new Emergency(), List.of(firstDate, secondDate));
            System.out.printf("Toll Fee: %d", fee);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
