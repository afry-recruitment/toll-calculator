import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

public class SampleData {
    
    /**
     * @return - Array of Date objects with the "yyyy-MM-dd HH:mm:ss" format
     */
    public Date[] getParkingSessions() {

        SimpleDateFormat simpleDateFormat = new SimpleDateFormat ("yyyy-MM-dd HH:mm:ss");

        try {
            Date[] dates = {
                simpleDateFormat.parse("2022-09-12 05:05:05"),
                simpleDateFormat.parse("2022-09-12 06:06:06")
            };

            return dates;
        } catch (ParseException e) {
            e.printStackTrace();
            return new Date[]{};
        }
    }
}
