
using ConsoleClient.Repo;
using DataLib.Enum;

ApiRepo repo = new ApiRepo();
var s = await repo.GetData(Vehicles.Car);
Console.WriteLine($"{s.Id} {s.Type}");