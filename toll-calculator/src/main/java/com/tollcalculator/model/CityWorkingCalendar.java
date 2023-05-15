package com.tollcalculator.model;

import jakarta.persistence.*;
import lombok.*;

@Getter
@Setter
@Entity
@NoArgsConstructor
@AllArgsConstructor
@Builder
@Table(name = "city_working_calendar")
public class CityWorkingCalendar {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @OneToOne
    @MapsId
    @JoinColumn(name = "city_id")
    private City city;

    @Column(name = "is_monday")
    private boolean isMonday;

    @Column(name = "is_tuesday")
    private boolean isTuesday;

    @Column(name = "is_wednesday")
    private boolean isWednesday;

    @Column(name = "is_thursday")
    private boolean isThursday;

    @Column(name = "is_friday")
    private boolean isFriday;

    @Column(name = "is_saturday")
    private boolean isSaturday;

    @Column(name = "is_sunday")
    private boolean isSunday;
}
