package com.afry.test.model;

import javax.persistence.*;
import java.util.List;
import java.util.Objects;

/**
 * DTO Mapping for Vehicle Entity this is abstract level implementation
 */
@Table(name = "vehicle")
@Entity
public abstract class Vehicle {
    @Id
    private String vehicleId;
    @OneToMany(cascade= CascadeType.ALL, mappedBy="vehicle")
    private List<TollEntry> tollEntry;
    public List<TollEntry> getTollEntries() {
        return tollEntry;
    }
    public void setTollEntries(List<TollEntry> tollEntry) {
        this.tollEntry = tollEntry;
    }
    public String getVehicleId() {
        return vehicleId;
    }
    public void setVehicleId(String vehicleId) {
        this.vehicleId = vehicleId;
    }
    public List<TollEntry> getTollEntry() {
        return tollEntry;
    }

    public void setTollEntry(List<TollEntry> tollEntry) {
        this.tollEntry = tollEntry;
    }
    public abstract String getType();

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (!(o instanceof Vehicle)) return false;
        Vehicle vehicle = (Vehicle) o;
        return Objects.equals(getVehicleId(), vehicle.getVehicleId()) && Objects.equals(getTollEntry(), vehicle.getTollEntry());
    }

    @Override
    public int hashCode() {
        return Objects.hash(getVehicleId(), getTollEntry());
    }
}
