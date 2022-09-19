package com.afry.test.model;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;
import javax.persistence.Table;
import java.util.Objects;
@Table(name = "rush_hour")
@Entity
public class RushHour {
    @Id
    @GeneratedValue
    private Integer id;
    private Integer fromHour;
    private Integer toHour;

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }
    public Integer getFromHour() {
        return fromHour;
    }
    public void setFromHour(Integer fromHour) {
        this.fromHour = fromHour;
    }
    public Integer getToHour() {
        return toHour;
    }
    public void setToHour(Integer toHour) {
        this.toHour = toHour;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (!(o instanceof RushHour)) return false;
        RushHour rushHour = (RushHour) o;
        return Objects.equals(getId(), rushHour.getId()) && Objects.equals(getFromHour(), rushHour.getFromHour()) && Objects.equals(getToHour(), rushHour.getToHour());
    }

    @Override
    public int hashCode() {
        return Objects.hash(getId(), getFromHour(), getToHour());
    }
}
