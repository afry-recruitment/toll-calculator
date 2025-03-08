// See https://aka.ms/new-console-template for more information
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaxApp;


  var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build())
            .AddSingleton<ITaxService, TaxCalculator>()
            .AddSingleton<IDbApi, DbApi>()
            .BuildServiceProvider();

var taxCalculatorService = serviceProvider.GetService<ITaxService>();

//Some different vehicles
IVehicle aCar = new Car();
IVehicle aMotorCycle = new MotorCycle();


if (taxCalculatorService != null){
    var configuration = serviceProvider.GetService<IConfiguration>();
    
    int cityId;
    if (configuration == null || !int.TryParse(configuration.GetSection("City").Value, out cityId))
    {
        Console.WriteLine("Invalid City in configuration.");
        return;
    }

    var fivePassagesCar = taxCalculatorService.GetTax(new Car(), GetFivePassages(), cityId: cityId);
    var frequentCar = taxCalculatorService.GetTax(new Car(), OneFrequentDay(), cityId: 1);
    var fivePassagesWithinOneHour = taxCalculatorService.GetTax(new Car(), MultiPassRoleFivePassages(), cityId: cityId);
    var tenDaysFivepassagesEach = taxCalculatorService.GetTax(aCar, GetTenDaysWithFivePassagesEach(), cityId: cityId);
    var mcTaxToPay = taxCalculatorService.GetTax(aMotorCycle, GetRandomCongestionPassages(), cityId: cityId);

    Console.WriteLine($"The five passages during that day costs {fivePassagesCar} swedish crowns");
    Console.WriteLine($"The frequent car has to pay {frequentCar} swedish crowns");
    Console.WriteLine($"Five passages within one hour has to pay {fivePassagesWithinOneHour} swedish crowns");
    Console.WriteLine($"The random passanger needs to pay {tenDaysFivepassagesEach} swedish crowns");
    Console.WriteLine($"mcTaxToPay needs to pay {mcTaxToPay} swedish crowns");
}
else{
    Console.WriteLine("Tax calculator service is not available.");
}


/*
    Five passages, with two free dates. The known sum of 47 crowns
    0, 0, 22, 16, 9 = 47
*/

List<DateTime> GetFivePassages()
{
    return new List<DateTime>
    {
        DateTime.Parse("2025-01-01 15:39:27"),
        DateTime.Parse("2025-01-06 10:12:19"),
        DateTime.Parse("2025-01-07 15:39:27"),
        DateTime.Parse("2025-02-14 17:39:00"),
        DateTime.Parse("2025-02-20 09:05:21")
    };
}

/*
    More than 60 crowns to pay for a day = max 60 crowns.
*/

List<DateTime> OneFrequentDay()
{
    return new List<DateTime>
    {
        DateTime.Parse("2025-03-05 09:10:00"),
        DateTime.Parse("2025-03-05 09:45:00"),
        DateTime.Parse("2025-03-05 10:30:00"),
        DateTime.Parse("2025-03-05 11:15:00"),
        DateTime.Parse("2025-03-05 12:00:00"),
        DateTime.Parse("2025-03-05 12:45:00"),
        DateTime.Parse("2025-03-05 13:30:00"),
        DateTime.Parse("2025-03-05 14:15:00"),
        DateTime.Parse("2025-03-05 15:00:00"),
        DateTime.Parse("2025-03-05 15:45:00"),
        DateTime.Parse("2025-03-05 16:00:00"),
        DateTime.Parse("2025-03-05 16:15:00"),
        DateTime.Parse("2025-03-05 16:30:00"),
        DateTime.Parse("2025-03-05 16:45:00"),
        DateTime.Parse("2025-03-05 18:17:00")
    };
}

/*
    Five passages within two different taxes. The highest one should be charged.
    In this case = 22 kr (07:00 - 07:59)
*/

List<DateTime> MultiPassRoleFivePassages()
{
    return new List<DateTime>
    {
        DateTime.Parse("2025-03-05 06:26:00"),
        DateTime.Parse("2025-03-05 06:45:00"),
        DateTime.Parse("2025-03-05 07:00:00"),
        DateTime.Parse("2025-03-05 07:15:00"),
        DateTime.Parse("2025-03-05 07:25:00")
    };
}

/*
    Just some fake data to play with.
*/
List<DateTime> GetRandomCongestionPassages()
{
    List<DateTime> randomDates = new List<DateTime>();
    Random rnd = new Random();
    DateTime start = new DateTime(2025, 1, 1);
    DateTime end = DateTime.Now;

    // Ensure at least three datetimes on the same day
    DateTime sameDay = start.AddDays(rnd.Next((end - start).Days));
    for (int i = 0; i < 3; i++)
    {
        randomDates.Add(sameDay.AddHours(rnd.Next(0, 24)).AddMinutes(rnd.Next(0, 60)).AddSeconds(rnd.Next(0, 60)));
    }

    // Generate the remaining random dates
    for (int i = 3; i < 15; i++)
    {
        int range = (end - start).Days;
        randomDates.Add(start.AddDays(rnd.Next(range)).AddHours(rnd.Next(0, 24)).AddMinutes(rnd.Next(0, 60)).AddSeconds(rnd.Next(0, 60)));
    }

    return randomDates;
}

/*
    Generate a list of datetimes that contains 10 different days with at least 5 datetimes on each day.
*/
List<DateTime> GetTenDaysWithFivePassagesEach()
{
    List<DateTime> dates = new List<DateTime>();
    Random rnd = new Random();
    DateTime start = new DateTime(2025, 1, 1);
    DateTime end = DateTime.Now;

    for (int i = 0; i < 10; i++)
    {
        DateTime day = start.AddDays(rnd.Next((end - start).Days));
        for (int j = 0; j < 5; j++)
        {
            dates.Add(day.AddHours(rnd.Next(0, 24)).AddMinutes(rnd.Next(0, 60)).AddSeconds(rnd.Next(0, 60)));
        }
    }

    return dates;
}

