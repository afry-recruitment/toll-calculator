import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class TollTimesAndFees {
    public static final int maxTotal = 60;
    private int lowTrafficPrice;
    private int midTrafficPrice;
    private int rushHourPrice;
    private List<TollTimeSlot> tollTimesAndFeesList = new ArrayList<>();

    public List<TollTimeSlot> TollTimesAndFees() {
        lowTrafficPrice = 8;
        midTrafficPrice = 13;
        rushHourPrice = 18;

        tollTimesAndFeesList.addAll(Arrays.asList(
                (new TollTimeSlot(6, 0, 6, 29, lowTrafficPrice)),
                (new TollTimeSlot(6, 30, 6, 59, midTrafficPrice)),
                (new TollTimeSlot(7, 0, 7, 59, rushHourPrice)),
                (new TollTimeSlot(8, 0, 8, 29, midTrafficPrice)),
                (new TollTimeSlot(8, 30, 14, 59, lowTrafficPrice)),
                (new TollTimeSlot(15, 0, 15, 29, midTrafficPrice)),

                //Rush hour price below beginning at 15.30. Assumed mistake when starting at 15.00.
                (new TollTimeSlot(15, 30, 16, 59, rushHourPrice)),
                (new TollTimeSlot(17, 0, 17, 59, midTrafficPrice)),
                (new TollTimeSlot(18, 0, 18, 29, lowTrafficPrice))
        ));

        return tollTimesAndFeesList;
    }
}
