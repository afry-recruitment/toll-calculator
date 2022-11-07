public class TollTimeSlot {
    private int startHour;
    private int startMinute;
    private int endHour;
    private int endMinute;
    private int fee;

    public TollTimeSlot(int startHour, int startMinute, int endHour, int endMinute, int fee) {
        this.startHour = startHour;
        this.startMinute = startMinute;
        this.endHour = endHour;
        this.endMinute = endMinute;
        this.fee = fee;
    }

    public int getStartHour() {
        return startHour;
    }

    public int getStartMinute() {
        return startMinute;
    }

    public int getEndHour() {
        return endHour;
    }

    public int getEndMinute() {
        return endMinute;
    }

    public int getFee() {
        return fee;
    }
}
