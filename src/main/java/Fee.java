import java.time.LocalTime;
import java.util.ArrayList;
import java.util.List;

public class Fee {
    public int getFee(LocalTime time) {

        int hour = time.getHour();
        int minute = time.getMinute();

        TollTimesAndFees timesAndFees = new TollTimesAndFees();

        List<TollTimeSlot> timeSlots = new ArrayList<>(timesAndFees.TollTimesAndFees());

        for (TollTimeSlot slot : timeSlots) {
            if ((hour >= slot.getStartHour() && minute >= slot.getStartMinute())
                    && (hour <= slot.getEndHour() && minute <= slot.getEndMinute()))
                return slot.getFee();
        }
        return 0;
    }
}