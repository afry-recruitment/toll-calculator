import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;

public class RushHoursDatabase {

    public ArrayList<HashMap<String, Integer>> getTollFeeAtPeakTimes() {

        HashMap<String, Integer> rushHour1 = new HashMap<String, Integer>();
        rushHour1.put("from", 600);
        rushHour1.put("to", 629);
        rushHour1.put("result", 8);

        HashMap<String, Integer> rushHour2 = new HashMap<String, Integer>();
        rushHour2.put("from", 630);
        rushHour2.put("to", 659);
        rushHour2.put("result", 13);

        HashMap<String, Integer> rushHour3 = new HashMap<String, Integer>();
        rushHour3.put("from", 700);
        rushHour3.put("to", 759);
        rushHour3.put("result", 18);

        HashMap<String, Integer> rushHour4 = new HashMap<String, Integer>();
        rushHour4.put("from", 800);
        rushHour4.put("to", 829);
        rushHour4.put("result",13);

        HashMap<String, Integer> rushHour5 = new HashMap<String, Integer>();
        rushHour5.put("from", 830);
        rushHour5.put("to", 859);
        rushHour5.put("result", 8);

        HashMap<String, Integer> rushHour6 = new HashMap<String, Integer>();
        rushHour6.put("from", 150);
        rushHour6.put("to", 1529);
        rushHour6.put("result", 13);

        HashMap<String, Integer> rushHour7 = new HashMap<String, Integer>();
        rushHour7.put("from", 1530);
        rushHour7.put("to", 1659);
        rushHour7.put("result", 18);

        HashMap<String, Integer> rushHour8 = new HashMap<String, Integer>();
        rushHour8.put("from", 170);
        rushHour8.put("to", 1759);
        rushHour8.put("result", 13);

        HashMap<String, Integer> rushHour9 = new HashMap<String, Integer>();
        rushHour9.put("from", 180);
        rushHour9.put("to", 1829);
        rushHour9.put("result", 8);


        ArrayList<HashMap<String, Integer>> options = new ArrayList<HashMap<String, Integer>>();
        options.add(rushHour1);
        options.add(rushHour2);
        options.add(rushHour3);
        options.add(rushHour4);
        options.add(rushHour5);
        options.add(rushHour6);
        options.add(rushHour7);
        options.add(rushHour8);
        options.add(rushHour9);

        return options;
    }

    public int getTollFeeAtPeakTimesCauculus(final Date date, Vehicle vehicle) {

        TollCalculator tollCalculator = new TollCalculator();

        if(Boolean.TRUE.equals(tollCalculator.isTollFreeDate(date)) || tollCalculator.isTollFreeVehicle(vehicle)) return 0;
        Calendar calendar = Calendar.getInstance();
        calendar.setTime(date);
        int hour = calendar.get(Calendar.HOUR_OF_DAY);
        int minute = calendar.get(Calendar.MINUTE);

        int hourMinute = Integer.parseInt(String.format("%d%d", hour, minute));
        if(hourMinute < 100) {
            hourMinute = hour * 100;
        }
        int finalResult = 0;

        for (HashMap<String, Integer> option : getTollFeeAtPeakTimes()) {
            int from  = option.get("from");
            int to  = option.get("to");
            int result  = option.get("result");

            if (hourMinute >= from && hourMinute <= to) {
                finalResult = result;
                break;
            }
        }
        return finalResult;

    }

}
