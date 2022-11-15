using TollCalculator.Models;
using TollCalculator.ViewModels;


Console.ForegroundColor = ConsoleColor.Yellow;

bool closeApplication = false;

while (closeApplication is false)
{
    Console.WriteLine($"Select an vehicle:");

    var vehicles = new List<IVehicle>()
    {
        new Car(),
        new MilitaryVehicle(),
        new DiplomatVehicle(),
        new EmergencyVehicle(),
        new Motorbike(),
        new Tractor()
    };

    int amountToPay = 0;

    for (int i = 0; i < vehicles.Count; i++)
    {
        IVehicle? vehicle = vehicles[i];
        Console.WriteLine($"{i}. {vehicle.GetType().Name}");
    }

    if (int.TryParse(Console.ReadLine(), out int selectedVehicle))
    {
        switch (selectedVehicle)
        {
            case 0:
                amountToPay = GetTotalFee(new Car());
                break;
            case 1:
                amountToPay = GetTotalFee(new MilitaryVehicle());
                break;
            case 2:
                amountToPay = GetTotalFee(new DiplomatVehicle());
                break;
            case 3:
                amountToPay = GetTotalFee(new EmergencyVehicle());
                break;
            case 4:
                amountToPay = GetTotalFee(new Motorbike());
                break;
            case 5:
                amountToPay = GetTotalFee(new Tractor());
                break;
            default:
                Console.WriteLine("Please select an vehicle.");
                break;
        }

        Console.WriteLine($"Your fee is: {amountToPay} Kr (SEK)");
        Console.WriteLine("Press escape to quit application...");

        var selectedOption = Console.ReadKey();

        if (selectedOption.Key is ConsoleKey.Escape)
        {
            closeApplication = true;
        }
        Console.Clear();
    }
}

static int GetTotalFee(IVehicle vehicle)
{
    var dateTimeNow = DateTime.Now;

    Console.WriteLine($"Setting time of tollpass: {dateTimeNow.ToString("G")}");

    var tollPassVehicle = new TollVehicleViewModel
    {
        Vehicle = vehicle,
        TollPassesDuringDay = new DateTime[]
        {
           dateTimeNow
        }
    };

    Console.WriteLine("To provide an custom time you may enter one now(Tip copy date above to match the correct format), else leave empty:");

    string input = Console.ReadLine();

    if (string.IsNullOrEmpty(input))
       return new TollCalculator.TollCalculator().GetTotalTollFeeByDay(tollPassVehicle);
    else
    {
        var customDate = GetCustomDate(input);
        tollPassVehicle.TollPassesDuringDay[0] = customDate;
    }

    Console.WriteLine("Add one more? Y (for yes): N (for no)");

    var option = Console.ReadLine();

    if (option.ToUpper() is "N")
    {
        var customDate = GetCustomDate(input);
        tollPassVehicle.TollPassesDuringDay[0] = customDate;
    }
    else if (option.ToUpper() is "Y")
    {
        bool askForMoreDates = true;

        while (askForMoreDates is true)
        {
            Console.WriteLine("Provide an additional:");

            input = Console.ReadLine();

            var additionalCustomDate = GetCustomDate(input);

            var previousCustomDates = tollPassVehicle.TollPassesDuringDay;

            var amountOfProvidedDates = previousCustomDates.Length;

            tollPassVehicle.TollPassesDuringDay = new DateTime[amountOfProvidedDates + 1];

            for (int i = 0; i < tollPassVehicle.TollPassesDuringDay.Length; i++)
            {
                if(i < previousCustomDates.Length)
                    tollPassVehicle.TollPassesDuringDay[i] = previousCustomDates[i];
                else
                {
                    tollPassVehicle.TollPassesDuringDay[i] = additionalCustomDate;
                }                
            }

            Console.WriteLine("Add one more? Y (for yes): N (for no)");
            option = Console.ReadLine();

            if (option.ToUpper() is "Y")
            {
                askForMoreDates = true;
            }
            else
            {
                askForMoreDates = false;
            }
        }
    }
  
    return new TollCalculator.TollCalculator().GetTotalTollFeeByDay(tollPassVehicle);
}

static DateTime GetCustomDate(string input)
{
    if (DateTime.TryParse(input, out DateTime customDate))
    {
        return customDate;
    }
    else
    {
        customDate = DateTime.Now;
        Console.WriteLine("Invalid date provided. Using datagenerated instead...");
    }

    return customDate;
}

