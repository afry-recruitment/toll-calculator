using TollFeeCalculator;

DateTime[] dateTimes1 = {
    new DateTime(2023, 09, 29, 12, 0, 0),
};
DateTime[] dateTimes2 = {
    new DateTime(2023, 09, 29, 12, 0, 0),
    new DateTime(2023, 09, 29, 12, 30, 0)
};
DateTime[] dateTimes3 = {
    new DateTime(2023, 09, 29, 12, 0, 0),
    new DateTime(2023, 09, 29, 12, 30, 0),
    new DateTime(2023, 09, 29, 13, 30, 0),
    new DateTime(2023, 09, 29, 13, 45, 0)
};

Test(dateTimes1);
Test(dateTimes2);
Test(dateTimes3);

void Test(DateTime[] dates)
{
    foreach (DateTime dt in dates)
    {
        Console.WriteLine("Date: " + dt.ToString() + "\n");
    }

    VehicleOld vehicleOld = new CarOld();
    Console.WriteLine("Old car type: " + vehicleOld.GetVehicleType());
    TollCalculatorOld tollCalculatorOld = new TollCalculatorOld();
    Console.WriteLine("Total cost (old): " + tollCalculatorOld.GetTollFee(vehicleOld, dates) + "\n");

    Vehicle vehicleNew = new Car();
    Console.WriteLine("New car type: " + vehicleNew.GetVehicleType());
    TollCalculator tollCalculatorNew = new TollCalculator();
    Console.WriteLine("Total cost (new): " + tollCalculatorNew.GetTollFee(vehicleNew, dates));
    Console.WriteLine("---------");
}