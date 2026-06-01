package schedules;

import java.time.LocalDateTime;

public class StandardSchedule implements Schedule {

    @Override
    public int getTollFee(LocalDateTime ldt) {
        int hour = ldt.getHour();
        int minute = ldt.getMinute();

        if (hour == 6)
            return minute < 30 ? 8 : 13;
        if (hour == 7)
            return 18;
        if (hour == 8)
            return minute < 30 ? 13 : 8;
        if (hour == 15)
            return minute < 30 ? 13 : 18;
        if (hour == 16)
            return 18;
        if (hour == 17)
            return 13;
        if (hour == 18 && minute < 30)
            return 8;
        return 0;
    }

}
