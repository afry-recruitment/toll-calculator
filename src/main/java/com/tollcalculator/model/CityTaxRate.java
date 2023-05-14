package com.tollcalculator.model;

import jakarta.persistence.*;
import lombok.*;

import java.math.BigDecimal;
import java.time.LocalTime;

@Getter
@Setter
@Entity
@NoArgsConstructor
@AllArgsConstructor
@Builder
@Table(name = "city_tax_rate")
public class CityTaxRate {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(name="charge")
    private BigDecimal charge;

    @Column(name="from_time")
    private LocalTime fromTime;

    @Column(name="to_time")
    private LocalTime toTime;

    @ManyToOne
    @JoinColumn(name = "city_id")
    private City city;
}
