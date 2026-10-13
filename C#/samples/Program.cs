using TollFeeCalculator;

var tollCalculator = new TollCalculator();
var car = new Car();

var dates = new DateTime[4];
dates[0] = new DateTime(2013, 10, 11, 6, 30, 0, 0);
dates[1] = new DateTime(2013, 10, 11, 7, 30, 0, 0);
dates[2] = new DateTime(2013, 10, 11, 8, 30, 0, 0);
dates[3] = new DateTime(2013, 10, 11, 9, 30, 0, 0);

var result = tollCalculator.GetTollFee(car, dates);
Console.WriteLine($"result: {result}");
