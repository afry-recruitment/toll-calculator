package com.afry.test.model;

import javax.persistence.*;
import java.util.Date;
import java.util.Objects;
/**
 * DTO Mapping for TollEntry
 */
@Table(name = "toll_entry")
@Entity
public class TollEntry {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;
    @ManyToOne(fetch = FetchType.LAZY, optional = false)
    @JoinColumn(name = "vehicle_id")
    private Vehicle vehicle;
    private Date entryDate;
    public Date getEntryDate() {
        return entryDate;
    }
    public void setEntryDate(Date entryDate) {
        this.entryDate = entryDate;
    }

    public Vehicle getVehicle() {
        return vehicle;
    }

    public void setVehicle(Vehicle vehicle) {
        this.vehicle = vehicle;
    }
    public Integer getId() {
        return id;
    }
    public void setId(Integer id) {
        this.id = id;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (!(o instanceof TollEntry)) return false;
        TollEntry tollEntry = (TollEntry) o;
        return Objects.equals(getId(), tollEntry.getId()) && Objects.equals(getVehicle(), tollEntry.getVehicle()) && Objects.equals(getEntryDate(), tollEntry.getEntryDate());
    }

    @Override
    public int hashCode() {
        return Objects.hash(getId(), getVehicle(), getEntryDate());
    }
}
