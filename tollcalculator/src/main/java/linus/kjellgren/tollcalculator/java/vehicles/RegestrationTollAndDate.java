/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package linus.kjellgren.tollcalculator.java.vehicles;

import java.time.Instant;
import java.time.LocalDateTime;
import java.time.ZoneOffset;
import java.time.format.DateTimeFormatter;
import java.time.format.FormatStyle;
import java.util.ArrayList;
import java.util.Date;
import java.util.ListIterator;
import javafx.beans.property.SimpleStringProperty;

/**
 *
 * @author linus
 */
public class RegestrationTollAndDate {
    private SimpleStringProperty registration;
    private int toll;
    private Vehicle vehicleType;
    private String vehicleTypeString;
    private ArrayList<Date> dates;
    private String datesString;

    public RegestrationTollAndDate(String registration, int toll, Vehicle vehicleType, ArrayList<Date> dates) {
        this.registration = new SimpleStringProperty(registration);
        this.toll = toll;
        this.vehicleType = vehicleType;
        this.dates = dates;
    }
    public String getVehicleTypeString(){
        return vehicleType.getType();
    }
    public Vehicle getVehicleType() {
        return vehicleType;
    }

    public void setVehicleType(Vehicle vehicleType) {
        this.vehicleType = vehicleType;
    }

    
    public String getRegistration() {
        return registration.get();
    }

    public void setRegistration(String registration) {
        this.registration = new SimpleStringProperty(registration);
    }

    public String getDatesString() {
        StringBuffer out = new StringBuffer();
        ListIterator<Date> iterator = this.dates.listIterator();
        while(iterator.hasNext()){
            Date tempDate = iterator.next();
            Instant instant = tempDate.toInstant();
            LocalDateTime localDateTime =  instant.atOffset(ZoneOffset.UTC).toLocalDateTime();
            out.append(localDateTime.format(DateTimeFormatter.ofPattern("yyyy-MM-dd 'kl.' HH:mm:ss")));
            if (iterator.hasNext()){
                out.append(System.getProperty("line.separator"));
            }
        }
        return out.toString();
    }
    
    
    
    
    public int getToll() {
        return toll;
    }

    public void setToll(int toll) {
        this.toll = toll;
    }

    public ArrayList<Date> getDates() {
        return dates;
    }

    public void setDates(ArrayList<Date> dates) {
        this.dates = dates;
    }

    
    
    
}
