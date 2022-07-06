using System;

namespace TollFeeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            { 
            int tollfees = 0;
            Vehicle vehicle;
            DateTime[] dateTimes = new DateTime[]
                                        {
                                            // DateTime.Now.AddHours(-6),DateTime.Now.AddHours(-6).AddMinutes(30),DateTime.Now.AddHours(-6).AddMinutes(60)
                                            //,DateTime.Now.AddHours(-6).AddMinutes(90)

                                            DateTime.Now,DateTime.Now.AddMinutes(30),DateTime.Now.AddMinutes(60),DateTime.Now.AddMinutes(90)
                                            
                                        };
            Console.WriteLine("Please enter vehicle type.");
            string vehicleType = Console.ReadLine().ToString();
            switch (vehicleType)
            {
                case "Motorbike":
                    TollFeeFreeVehicleMesg(vehicleType);
                    break;
                case "Tractor":
                    TollFeeFreeVehicleMesg(vehicleType);
                    break;
                case "Emergency":
                    TollFeeFreeVehicleMesg(vehicleType);
                    break;
                case "Diplomat":
                    TollFeeFreeVehicleMesg(vehicleType);
                    break;
                case "Foreign":
                    TollFeeFreeVehicleMesg(vehicleType);
                    break;
                case "Military":
                    TollFeeFreeVehicleMesg(vehicleType);
                    break;
                case "Car":
                    TollfeeCalculationMesg(vehicleType);
                    vehicle = new Car();
                    tollfees = TollCalculator.GetTollFee(vehicle, dateTimes);
                    Console.WriteLine(string.Format("Total calculated toll fee is :{0} SEK.", tollfees.ToString()));
                    break;

                default:
                    Console.WriteLine("Vehicle type is not found");
                    break;
            }
                Console.WriteLine("Press enter to calculate toll fee for another vehicle.");
            Console.ReadKey();
            }
        }
        private static void TollFeeFreeVehicleMesg(string vehicleTye)
        {
            Console.WriteLine("Toll fee is exempted for " + vehicleTye + " vehicle.");
        }
        private static void TollfeeCalculationMesg(string vehicleTye)
        {
            Console.WriteLine("Calculating toll fee for " + vehicleTye + "vehicle.");
        }

    }
}
