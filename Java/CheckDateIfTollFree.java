import java.util.*;

public class CheckDateIfTollFree {
    
    Calendar calendar = Calendar.getInstance();

    private List<Integer> jan = new ArrayList<>(Arrays.asList(new Integer[]{1}));
    private List<Integer> feb = new ArrayList<>(Arrays.asList(new Integer[]{}));
    private List<Integer> mar = new ArrayList<>(Arrays.asList(new Integer[]{28,29}));
    private List<Integer> apr = new ArrayList<>(Arrays.asList(new Integer[]{1,30}));
    private List<Integer> may = new ArrayList<>(Arrays.asList(new Integer[]{1,8,9}));
    private List<Integer> jun = new ArrayList<>(Arrays.asList(new Integer[]{5,6,21}));
    private List<Integer> jul = new ArrayList<>(Arrays.asList(new Integer[]{1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31}));
    private List<Integer> aug = new ArrayList<>(Arrays.asList(new Integer[]{}));
    private List<Integer> sep = new ArrayList<>(Arrays.asList(new Integer[]{}));
    private List<Integer> oct = new ArrayList<>(Arrays.asList(new Integer[]{}));
    private List<Integer> nov = new ArrayList<>(Arrays.asList(new Integer[]{1}));
    private List<Integer> dec = new ArrayList<>(Arrays.asList(new Integer[]{24,25,26,31}));

    private List<Integer> dayOfWeek = new ArrayList<>(Arrays.asList(new Integer[]{Calendar.SATURDAY,Calendar.SUNDAY}));

    public CheckDateIfTollFree(Date date) {
        calendar.setTime(date);
    }

    public boolean checkIfTollFree() {
        if (isDateTollFree() || isDayTollFree()) {
            return true;
        } else {
            return false;
        }
    }

    private boolean isDateTollFree() {

        switch(calendar.get(Calendar.MONTH)) {
            case 0: return jan.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 1: return feb.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 2: return mar.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 3: return apr.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 4: return may.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 5: return jun.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 6: return jul.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 7: return aug.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 8: return sep.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 9: return oct.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 10: return nov.contains(calendar.get(Calendar.DAY_OF_MONTH));
            case 11: return dec.contains(calendar.get(Calendar.DAY_OF_MONTH));
            default: return false;
        }
    }

    private boolean isDayTollFree() {
        return dayOfWeek.contains(calendar.get(Calendar.DAY_OF_WEEK));
    }
}
