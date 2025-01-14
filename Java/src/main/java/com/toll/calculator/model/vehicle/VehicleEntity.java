package com.toll.calculator.model.vehicle;

import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;

@Entity
@Table(name = "vehicles")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
public class VehicleEntity implements Vehicle{

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(nullable = false, unique = true)
    private String regNumber;

    @ManyToOne(optional = false, fetch = FetchType.EAGER)
    @JoinColumn(name = "vehicle_type_id", referencedColumnName = "id", nullable = false)
    private VehicleType vehicleType;

    @ElementCollection
    @CollectionTable(name = "toll_dates", joinColumns = @JoinColumn(name = "vehicle_id"))
    @Column(name = "crossed_date")
    private List<Date> tollDates = new ArrayList<>();

}
