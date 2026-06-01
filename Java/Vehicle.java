import java.time.LocalDateTime;

import schedules.Schedule;
import schedules.StandardSchedule;

public enum Vehicle {

    MOTORBIKE("Motorbike"),
    TRACTOR("Tractor"),
    EMERGENCY("Emergency"),
    DIPLOMAT("Diplomat"),
    FOREIGN("Foreign"),
    CAR("Car", false),
    MILITARY("Military");

    private final String name;
    private final Schedule schedule;
    private boolean tollFree;

    public boolean isTollFree() {
        return tollFree;
    }

    Vehicle(String name, boolean tollFree, Schedule schedule) {
        this.name = name;
        this.tollFree = tollFree;
        this.schedule = schedule;
    }

    Vehicle(String name, boolean tollFree) {
        Schedule schedule;
        if (!tollFree) {
            schedule = new StandardSchedule();
        } else {
            schedule = new Schedule() {
                @Override
                public int getTollFee(LocalDateTime ldt) {
                    return 0;
                }
            };
        }
        this(name, tollFree, schedule);
    }

    Vehicle(String name) {
        this(name, true);
    }

    public int getTollFee(LocalDateTime ldt) {
        return schedule.getTollFee(ldt);
    }

    public String getName() {
        return name;
    }
}
