namespace TollFeeCalculator
{

    public class Test
    {
        //Test passager, år, dag, månad ,timme, minut, sekund. 
        //Dates är information om passeringar för en viss bil.
        //Om man vill läsa flera olika bilar i samma dates, får vi ha källdata med någon nyckel att gruppera dom på.
        //som te.x. reg nummer. 

        DateTime[] dates =
        {
            new DateTime(2026, 11, 02, 06, 45, 0),
            new DateTime(2026, 11, 02, 07, 58, 0),

            new DateTime(2026, 11, 03, 06, 45, 0),
            new DateTime(2026, 11, 03, 07, 58, 0),

            //new DateTime(2026, 11, 01, 18, 15, 0),
            //new DateTime(2026, 11, 01, 18, 50, 0),
            //new DateTime(2026, 11, 02, 18, 15, 0),
            //new DateTime(2026, 11, 02, 18, 45, 0),
            //new DateTime(2026, 11, 02, 20, 45, 0),
            //new DateTime(2026, 11, 03, 07, 00, 0),
            //new DateTime(2026, 11, 03, 07, 50, 0),
            //new DateTime(2026, 11, 04, 07, 50, 0),
            //new DateTime(2026, 11, 05, 07, 50, 0),
        };

        Vehicle vehicle = new Vehicle(VehicleType.Car);

        TollCalculator calculator = new TollCalculator();

        public int GetFee()
        {
            int ret = calculator.GetTollFee(vehicle, dates);
            return ret;
        }
    }
}