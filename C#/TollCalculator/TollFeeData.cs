namespace TollFeeCalculator
{
    public class TollFee{
        public int Fee {get;}
        public TimeOnly StartTime{get;}

        public TimeOnly EndTime{get;}
        public TollFee(TimeOnly startTime, TimeOnly endTime,int fee){
            this.EndTime=endTime;
            this.StartTime=startTime;
            this.Fee=fee;
        }
    }
    public static class TollFeeData
    {

        //2012 official holidays
        public static List<DateOnly> TollFreeDates = new List<DateOnly>{
            new DateOnly(2012,1,1),
            new DateOnly(2012,4,14),
            new DateOnly(2012,4,15),
            new DateOnly(2012,4,18),
            new DateOnly(2012,5,25),
            new DateOnly(2012,5,26),
            new DateOnly(2012,6,6),
            new DateOnly(2012,6,24),
            
            new DateOnly(2012,11,4),
            new DateOnly(2012,12,24),
            new DateOnly(2012,12,26)
        };

        public static List<TollFee> TollFees= new List<TollFee>{
            new TollFee ( new TimeOnly(6, 0), new TimeOnly(6, 29), 8),
            new TollFee (new TimeOnly(6, 29), new TimeOnly(6, 59),13),
            new TollFee ( new TimeOnly(7, 0),  new TimeOnly(7, 59),18),
            new TollFee (new TimeOnly(8, 0),  new TimeOnly(8, 29),13),
            new TollFee (new TimeOnly(8, 30), new TimeOnly(14, 59),8),
            new TollFee (new TimeOnly(15, 0), new TimeOnly(15, 29),13),
            new TollFee ( new TimeOnly(15, 30),new TimeOnly(16, 59),18),
            new TollFee (new TimeOnly(17, 0), new TimeOnly(17, 59),13),
            new TollFee (new TimeOnly(18, 0), new TimeOnly(18, 30),8)
        };
    }
}