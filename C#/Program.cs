using System;
namespace TollFeeCalculator
{

    public class Program
    {
        static void Main(string[] args)
        {
           

            DateTime[] dateTimes = new DateTime[]
         {
            new DateTime(2024, 04, 03 ,06, 25, 0),//8
             new DateTime(2024, 04, 03 ,07, 15, 0),//18
            new DateTime(2024, 04, 03 ,08, 20, 0),//13
            new DateTime(2024, 04, 03 ,14, 00, 0),//8
            new DateTime(2024, 04, 03 ,15, 15, 0),//13
            new DateTime(2024, 04, 03 ,15, 35, 0),//18 
            new DateTime(2024, 04, 03 ,15, 45, 0),//18 
            new DateTime(2024, 04, 03 ,18, 10, 0),//8
            new DateTime(2024, 04, 03 ,18, 20, 0),//8
           };

            
           

            TollCalculator tc = new TollCalculator();
            Vehicle car = new Car();
            //Vehicle bike = new Motorbike();
            //Vehicle tractor = new Tractor();
            
            int fee=tc.GetTollFee(car, dateTimes);
                
            Console.WriteLine("Att betala: "+fee + " för "+ new DateTime(2024, 04, 03).ToShortDateString());

            Console.ReadLine();

            


        }
    }
}