using System;
using TollFeeCalculator;


//Testprogram för att testa så att det funkar. Skickar in olika passeringstider för att verifiera tidsintervallen, 60 min-regeln och maxbeloppet (60 SEK) per dag.
class Program
{
    static void Main()
    {
        var calculator = new TollCalculator();
        var vehicle = new Car();
        //var vehicleM = new Motorbike();

        DateTime[] passes =
        {

            new DateTime(2026,1,2,6,10,0),
            new DateTime(2026,1,2,6,20,0),
            new DateTime(2026,1,2,6,50,0)
            //new DateTime(2013,1,2,6,10,0),
            //new DateTime(2013,1,2,6,40,0)
            //new DateTime(2013,1,2,7,20,0),
            //new DateTime(2013,1,2,8,15,0)
        };

        int total = calculator.GetTollFee(vehicle, passes);

        Console.WriteLine("Total fee: " + total + " Vehicle type: " + vehicle.GetVehicleType());
        Console.ReadLine();
    }
}