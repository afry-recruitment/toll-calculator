import java.time.LocalDateTime;
import java.time.ZoneId;
import java.util.Date;

public class TestTime {

    public Date rushHourPriceTime1 = java.util.Date.from(LocalDateTime.of(
            2022, 11, 7, 7, 22).atZone(ZoneId.systemDefault()).toInstant());
    public Date midPriceTime = java.util.Date.from(LocalDateTime.of(
            2022, 11, 7, 8, 27).atZone(ZoneId.systemDefault()).toInstant());
    public Date lowPriceTime = java.util.Date.from(LocalDateTime.of(
            2022, 11, 7, 11, 17).atZone(ZoneId.systemDefault()).toInstant());
    public Date midPriceLessThanHourFromRush = java.util.Date.from(LocalDateTime.of(
            2022, 11, 7, 15, 27).atZone(ZoneId.systemDefault()).toInstant());
    public Date rushHourPriceTime2 = java.util.Date.from(LocalDateTime.of(
            2022, 11, 7, 16, 4).atZone(ZoneId.systemDefault()).toInstant());
    public Date midPriceTime2 = java.util.Date.from(LocalDateTime.of(
            2022, 11, 7, 17, 26).atZone(ZoneId.systemDefault()).toInstant());
    public Date noFeeTime = java.util.Date.from(LocalDateTime.of(
            2022, 11, 7, 18, 35).atZone(ZoneId.systemDefault()).toInstant());
    public Date nextDayTime = java.util.Date.from(LocalDateTime.of(
            2022, 11, 8, 7, 27).atZone(ZoneId.systemDefault()).toInstant());
}
