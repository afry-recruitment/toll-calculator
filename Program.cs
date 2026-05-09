// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

int day = 15;
int month = 5;
DateTime[] dates = [
        new DateTime(2026, month, day, 6, 15, 0),
        new DateTime(2026, month, day, 6, 45, 0)
    ];

var calc = new TollCalculator();
TollFeeCalculator.Car car = new TollFeeCalculator.Car();

int firstFee = calc.GetTollFee(car, dates);

var request = new TollFeeRequest
{
    Vehicle = new Car(),
    Passages = dates
};
var tollCalc = new TollCalc(new TollCalculatorService());
int secondFee = tollCalc.GetTollFeeByDayPassages(request);

Console.WriteLine("firstFee="+firstFee);
Console.WriteLine("secondFee="+secondFee);
