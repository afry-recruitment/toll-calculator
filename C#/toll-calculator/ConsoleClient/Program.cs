using ConsoleClient;
using ConsoleClient.Repo;

TollCalculator tollCalculator = new TollCalculator();

for (int i = 1; i <= 11; i++)
{
    foreach (var vehicle in InMemoryRepo.LoadVehicles())
    {
        switch (i)
        {
            case 1:
                Console.WriteLine($"Weekend: {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
            case 2:
                Console.WriteLine($"WeekDay: {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
            case 3:
                Console.WriteLine($"LongFriday: {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
            case 4:
                Console.WriteLine($"Weekday {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
            case 5:
                Console.WriteLine($"FirstDayOfMay: {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
            case 6:
                Console.WriteLine($"AscensionDay: {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
            case 7:
                Console.WriteLine($"EasterMonday: {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
            case 8:
                Console.WriteLine($"NatonalDay: {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
            case 9:
                Console.WriteLine($"Weekdays: {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
            case 10:
                Console.WriteLine($"ChristmessDays: {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
            case 11:
                Console.WriteLine($"Weekday: {vehicle.GetVehicleType()}: {vehicle.RegNumber} payed fee of {tollCalculator.GetTollFee(vehicle, InMemoryRepo.LoadNewDateAndTimes(i))} kr");
                break;
        }
    }
    Console.WriteLine();
}