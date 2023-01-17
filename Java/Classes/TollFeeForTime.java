package Classes;

public class TollFeeForTime {
    
    private int hourStart;
    private int hourEnd;
    private int minuteStart;
    private int minuteEnd;
    private int fee;

    public TollFeeForTime(int hourStart, int hourEnd, int minuteStart, int minuteEnd, int fee) {
        this.hourStart = hourStart;
        this.hourEnd = hourEnd;
        this.minuteStart = minuteStart;
        this.minuteEnd = minuteEnd;
        this.fee = fee;
    }

    public int getHourStart() {
        return hourStart;
    }

    public int getHourEnd() {
        return hourEnd;
    }

    public int getMinuteStart() {
        return minuteStart;
    }

    public int getMinuteEnd() {
        return minuteEnd;
    }

    public int getFee() {
        return fee;
    }
}
