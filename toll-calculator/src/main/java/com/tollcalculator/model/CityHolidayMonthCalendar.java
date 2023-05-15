package com.tollcalculator.model;

import jakarta.persistence.*;
import lombok.*;

@Getter
@Setter
@Entity
@NoArgsConstructor
@AllArgsConstructor
@Builder
@Table(name = "city_holiday_month_calendar")
public class CityHolidayMonthCalendar {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @OneToOne
    @MapsId
    @JoinColumn(name = "city_id")
    private City city;

    @Column(name = "is_january")
    private boolean isJanuary;

    @Column(name="is_february")
    private boolean isFebruary;

    @Column(name="is_march")
    private boolean isMarch;

    @Column(name = "is_april")
    private boolean isApril;

    @Column(name = "is_may")
    private boolean isMay;

    @Column(name="is_june")
    private boolean isJune;

    @Column(name="is_july")
    private boolean isJuly;

    @Column(name="is_august")
    private boolean isAugust;

    @Column(name="is_september")
    private boolean isSeptember;

    @Column(name = "is_october")
    private boolean isOctober;

    @Column(name = "is_november")
    private boolean isNovember;

    @Column(name="is_december")
    private boolean isDecember;
}
