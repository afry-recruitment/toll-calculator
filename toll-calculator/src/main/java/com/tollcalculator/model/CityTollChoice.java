package com.tollcalculator.model;

import jakarta.persistence.*;
import lombok.*;

@Getter
@Setter
@Entity
@NoArgsConstructor
@AllArgsConstructor
@Builder
@Table(name = "city_toll_choice")
public class CityTollChoice {

    @Id
    private Long id;

    @Column(name = "max_toll_per_day")
    private Integer maxTollPerDay;

    @Column(name = "number_of_toll_free_days_before_holiday")
    private Integer numberOfTollFreeDaysBeforeHoliday;

    @Column(name = "single_charge_interval_in_min")
    private Integer singleChargeIntervalInMin;

    @OneToOne
    @MapsId
    private City city;
}
