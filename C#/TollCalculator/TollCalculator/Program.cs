

using TollCalculator;
using TollCalculator.Interface;
using TollCalculator.Repos;

IVehicle vehicle = new Car();

DateTime[] passingDates =
{
    new DateTime(2023, 3, 12)  // red 0:- 
  , new DateTime(2023, 3, 13) // monday  9:-
};
passingDates[0] = passingDates[0].AddHours(6).AddMinutes(6);
passingDates[1] = passingDates[1].AddHours(12).AddMinutes(6);



int cost = global::TollCalculator.TollCalculator.GetTollFee(vehicle, passingDates);

Console.WriteLine("Kostnaden för de två datumen var: " +  cost);