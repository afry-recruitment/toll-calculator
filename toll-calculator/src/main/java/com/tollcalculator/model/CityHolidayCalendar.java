package com.tollcalculator.model;

import jakarta.persistence.*;
import lombok.*;

import java.util.Date;

@Getter
@Setter
@Entity
@NoArgsConstructor
@AllArgsConstructor
@Builder
@Table(name = "city_holiday_calendar")
public class CityHolidayCalendar {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name="date")
    private Date date;

    @ManyToOne
    @JoinColumn(name="city_id", nullable=false)
    private City city;
}
