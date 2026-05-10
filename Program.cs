Console.WriteLine("Hello, World!");

int day = 15;
int month = 5;
DateTime[] dates = [
        new DateTime(2026, month, day, 6, 15, 0),
        new DateTime(2026, month, day, 6, 45, 0),
        new DateTime(2026, month, day, 7, 05, 0),
        new DateTime(2026, month, day, 15, 10, 0)
    ];

// Gamla lösningen
var calc = new TollCalculator();
TollFeeCalculator.Car car = new TollFeeCalculator.Car();
int firstFee = calc.GetTollFee(car, dates);


// Här kommer nya lösningen
// Simulerar lite att det kommer som ett HTTP-anrop via rest-api endpoint.
var request = new TollFeeRequest
{
    Vehicle = new Car(),
    Passages = dates
};
// Inte snyggaste med DI här i instansieringen, men ville visa DI/service tänket.
var tollCalc = new TollCalc(new TollCalculatorService());
int secondFee = tollCalc.GetTollFeeByDayPassages(request);

Console.WriteLine("firstFee="+firstFee);
Console.WriteLine("secondFee="+secondFee);
