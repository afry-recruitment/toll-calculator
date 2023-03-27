
using ConsoleClient;

TollCalculator tollCalculator = new TollCalculator();

DateTime[] SaturDay = { DateTime.UtcNow,
                           new DateTime(2023, 3, 25, 9, 30, 0),
                           new DateTime(2023, 3, 25, 14, 0, 0),
                           new DateTime(2023, 3, 25, 18, 0, 0)
};
DateTime[] SunDay = { DateTime.UtcNow,
                           new DateTime(2023, 3, 26, 9, 30, 0),
                           new DateTime(2023, 3, 26, 14, 0, 0),
                           new DateTime(2023, 3, 26, 18, 0, 0)
};
DateTime[] weekDay = { DateTime.UtcNow,
                           new DateTime(2023, 3, 22, 9, 30, 0),
                           new DateTime(2023, 3, 22, 14, 0, 0),
                           new DateTime(2023, 3, 22, 18, 0, 0)

};
DateTime[] Christmass = { DateTime.UtcNow,
                           new DateTime(2023, 12, 24, 9, 30, 0),
                           new DateTime(2023, 12, 24, 14, 0, 0),
                           new DateTime(2023, 12, 24, 18, 0, 0)
};
DateTime[] Easter = { DateTime.UtcNow,
                           new DateTime(2023, 4, 8, 9, 30, 0),
                           new DateTime(2023, 4, 8, 14, 0, 0),
                           new DateTime(2023, 4, 8, 18, 0, 0)
};
DateTime[] TestMaxCost = { DateTime.UtcNow,
                           //new DateTime(2023, 3, 26, 22, 0, 0),
                          // new DateTime(2023, 3, 26, 23, 0, 0),
                           new DateTime(2023, 3, 27, 1, 0, 0),
                           new DateTime(2023, 3, 27, 2, 0, 0),
                           new DateTime(2023, 3, 27, 3, 0, 0),
                           new DateTime(2023, 3, 27, 4, 0, 0),
                           new DateTime(2023, 3, 27, 5, 0, 0),
                           new DateTime(2023, 3, 27, 6, 0, 0),
                           new DateTime(2023, 3, 27, 7, 0, 0),
                           new DateTime(2023, 3, 27, 8, 0, 0),
                           new DateTime(2023, 3, 27, 9, 0, 0),
                           new DateTime(2023, 3, 27, 10, 0, 0),
                           new DateTime(2023, 3, 27, 11, 0, 0),
                           new DateTime(2023, 3, 27, 12, 0, 0),
                           new DateTime(2023, 3, 27, 13, 0, 0),
                           new DateTime(2023, 3, 27, 14, 0, 0),

};

#region car
Console.WriteLine(tollCalculator.HolidaysBasedOnEaster(new DateTime(1989, 3, 27, 14, 0, 0)));
//Console.WriteLine(tollCalculator.GetDateOfEaster(2023));

/*Console.WriteLine("------------------Car-------------------------");
Console.WriteLine($"On {nameof(SaturDay)} - Total fee: "+tollCalculator.GetTollFee(new Car(), SaturDay));
Console.WriteLine($"On {nameof(Christmass)} - Total fee: " + tollCalculator.GetTollFee(new Car(), Christmass));
Console.WriteLine("On Weekday - Total fee: "+tollCalculator.GetTollFee(new Car(), weekDay));
Console.WriteLine($"On {nameof(SunDay)} - Total fee: " + tollCalculator.GetTollFee(new Car(), SunDay));
Console.WriteLine($"On {nameof(Easter)} - Total fee: " + tollCalculator.GetTollFee(new Car(), Easter));*/
//Console.WriteLine($"On {nameof(TestMaxCost)} - Total fee: " + tollCalculator.GetTollFee(new Car(), TestMaxCost));
#endregion
/*
#region MotorBike
Console.WriteLine("------------------Motorbike-------------------------");
Console.WriteLine($"On {nameof(SaturDay)} - Total fee: " + tollCalculator.GetTollFee(new Motorbike(), SaturDay));
Console.WriteLine($"On {nameof(Christmass)} - Total fee: " + tollCalculator.GetTollFee(new Motorbike(), Christmass));
Console.WriteLine("On Weekday - Total fee: " + tollCalculator.GetTollFee(new Motorbike(), weekDay));
Console.WriteLine($"On {nameof(SunDay)} - Total fee: " + tollCalculator.GetTollFee(new Motorbike(), SunDay));
Console.WriteLine($"On {nameof(Easter)} - Total fee: " + tollCalculator.GetTollFee(new Motorbike(), Easter));
Console.WriteLine($"On {nameof(TestMaxCost)} - Total fee: " + tollCalculator.GetTollFee(new Motorbike(), TestMaxCost));
#endregion
#region Tractor
Console.WriteLine("------------------Tractor-------------------------");
Console.WriteLine($"On {nameof(SaturDay)} - Total fee: " + tollCalculator.GetTollFee(new Tractor(), SaturDay));
Console.WriteLine($"On {nameof(Christmass)} - Total fee: " + tollCalculator.GetTollFee(new Tractor(), Christmass));
Console.WriteLine("On Weekday - Total fee: " + tollCalculator.GetTollFee(new Tractor(), weekDay));
Console.WriteLine($"On {nameof(SunDay)} - Total fee: " + tollCalculator.GetTollFee(new Tractor(), SunDay));
Console.WriteLine($"On {nameof(Easter)} - Total fee: " + tollCalculator.GetTollFee(new Tractor(), Easter));
Console.WriteLine($"On {nameof(TestMaxCost)} - Total fee: " + tollCalculator.GetTollFee(new Tractor(), TestMaxCost));

#endregion
#region Emergency
Console.WriteLine("------------------Emergency-------------------------");
Console.WriteLine($"On {nameof(SaturDay)} - Total fee: " + tollCalculator.GetTollFee(new Emergency(), SaturDay));
Console.WriteLine($"On {nameof(Christmass)}- Total fee: " + tollCalculator.GetTollFee(new Emergency(), Christmass));
Console.WriteLine("On Weekday - Total fee: " + tollCalculator.GetTollFee(new Emergency(), weekDay));
Console.WriteLine($"On {nameof(SunDay)} - Total fee: " + tollCalculator.GetTollFee(new Emergency(), SunDay));
Console.WriteLine($"On {nameof(Easter)} - Total fee: " + tollCalculator.GetTollFee(new Emergency(), Easter));
Console.WriteLine($"On {nameof(TestMaxCost)} - Total fee: " + tollCalculator.GetTollFee(new Emergency(), TestMaxCost));
#endregion
#region Diplomat
Console.WriteLine("------------------Diplomat-------------------------");
Console.WriteLine($"On {nameof(SaturDay)} - Total fee: " + tollCalculator.GetTollFee(new Diplomat(), SaturDay));
Console.WriteLine($"On {nameof(Christmass)} - Total fee: " + tollCalculator.GetTollFee(new Diplomat(), Christmass));
Console.WriteLine("On Weekday - Total fee: " + tollCalculator.GetTollFee(new Diplomat(), weekDay));
Console.WriteLine($"On {nameof(SunDay)} - Total fee: " + tollCalculator.GetTollFee(new Diplomat(), SunDay));
Console.WriteLine($"On {nameof(Easter)} - Total fee: " + tollCalculator.GetTollFee(new Diplomat(), Easter));
Console.WriteLine($"On {nameof(TestMaxCost)} - Total fee: " + tollCalculator.GetTollFee(new Diplomat(), TestMaxCost));
#endregion
#region Foreign
Console.WriteLine("------------------Foreign-------------------------");
Console.WriteLine($"On {nameof(SaturDay)} - Total fee: " + tollCalculator.GetTollFee(new Foreign(), SaturDay));
Console.WriteLine($"On {nameof(Christmass)} - Total fee: " + tollCalculator.GetTollFee(new Foreign(), Christmass));
Console.WriteLine("On Weekday - Total fee: " + tollCalculator.GetTollFee(new Foreign(), weekDay));
Console.WriteLine($"On {nameof(SunDay)} - Total fee: " + tollCalculator.GetTollFee(new Foreign(), SunDay));
Console.WriteLine($"On {nameof(Easter)} - Total fee: " + tollCalculator.GetTollFee(new Foreign(), Easter));
Console.WriteLine($"On {nameof(TestMaxCost)} - Total fee: " + tollCalculator.GetTollFee(new Foreign(), TestMaxCost));
#endregion
#region Military
Console.WriteLine("------------------Military-------------------------");
Console.WriteLine($"On {nameof(SaturDay)} - Total fee: " + tollCalculator.GetTollFee(new Military(), SaturDay));
Console.WriteLine($"On {nameof(Christmass)} - Total fee: " + tollCalculator.GetTollFee(new Military(), Christmass));
Console.WriteLine("On Weekday - Total fee: " + tollCalculator.GetTollFee(new Military(), weekDay));
Console.WriteLine($"On {nameof(SunDay)} - Total fee: " + tollCalculator.GetTollFee(new Military(), SunDay));
Console.WriteLine($"On {nameof(Easter)} - Total fee: " + tollCalculator.GetTollFee(new Military(), Easter));
Console.WriteLine($"On {nameof(TestMaxCost)} - Total fee: " + tollCalculator.GetTollFee(new Military(), TestMaxCost));
Console.WriteLine("-------------------------------------------------");
#endregion*/