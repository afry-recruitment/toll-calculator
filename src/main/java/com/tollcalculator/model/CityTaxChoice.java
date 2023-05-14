package com.tollcalculator.model;

import jakarta.persistence.*;
import lombok.*;

@Getter
@Setter
@Entity
@NoArgsConstructor
@AllArgsConstructor
@Builder
@Table(name = "city_tax_choice")
public class CityTaxChoice {

    @Id
    private Long id;

    @Column(name = "max_tax_per_day")
    private Integer maxTaxPerDay;

    @Column(name = "number_of_tax_free_days_before_holiday")
    private Integer numberOfTaxFreeDaysBeforeHoliday;

    @Column(name = "single_charge_interval_in_min")
    private Integer singleChargeIntervalInMin;

    @OneToOne
    @MapsId
    private City city;
}
