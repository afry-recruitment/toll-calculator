using TollFeeCalculator;


var calculator = new TollCalculator();
var vehicle = new Car();

var dates = new[]
{
    new DateTime(2013, 2, 4, 6, 15, 0),
    new DateTime(2013, 2, 4, 7, 16, 0)
};

var fee = calculator.GetTollFee(vehicle, dates);

Console.WriteLine($"Toll fee: {fee} SEK");
