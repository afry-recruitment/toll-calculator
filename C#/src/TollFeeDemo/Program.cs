using System.Globalization;
using TollFeeCalculator;

namespace TollFeeDemo;

public static class Program
{
    public static int Main(string[] args)
    {
        var calculator = new TollCalculator();

        if (args.Length == 1 && args[0] is "--help" or "-h")
        {
            PrintHelp();
            return 0;
        }

        try
        {
            if (args.Length > 0)
                RunCustomExample(calculator, args);
            else
                RunExamples(calculator);

            return 0;
        }
        catch (ArgumentException error)
        {
            Console.Error.WriteLine(error.Message);
            Console.Error.WriteLine("Use --help for the accepted input format.");
            return 2;
        }
    }

    private static void RunExamples(TollCalculator calculator)
    {
        Console.WriteLine("Toll fee calculator - 2013 exercise policy");
        Console.WriteLine("A chargeable passage starts a fixed 60-minute window.");
        Console.WriteLine("Exactly 60 minutes is included; free passages have no effect.");
        Console.WriteLine();

        PrintScenario(calculator, "Normal day", VehicleType.Car,
            new[] { At(6), At(6, 30), At(7), At(15, 30) },
            "The morning window costs 18 SEK; the afternoon window costs another 18.");

        PrintScenario(calculator, "Highest fee within 60 minutes", VehicleType.Car,
            new[] { At(6), At(6, 30), At(7) },
            "All three passages share one window, including the passage at exactly 07:00.");

        PrintScenario(calculator, "A passage after 60 minutes", VehicleType.Car,
            new[] { At(6), At(7, 1) },
            "Two windows: 8 + 18 = 26 SEK.");

        PrintScenario(calculator, "Higher fees do not extend the window", VehicleType.Car,
            new[] { At(6), At(6, 50), At(7, 40) },
            "06:50 raises the first window to 13 SEK. 07:40 starts a new one: total 31.");

        PrintScenario(calculator, "Daily maximum", VehicleType.Car,
            new[] { At(7), At(9), At(11), At(13), At(15, 30), At(17, 30) },
            "The windows add up to 73 SEK, reduced to the daily maximum of 60.");

        PrintScenario(calculator, "Exempt vehicle", VehicleType.Motorbike,
            new[] { At(7), At(15, 30) }, "Motorbikes are exempt, even during rush hour.");

        PrintScenario(calculator, "Weekend", VehicleType.Car,
            new[] { new DateTime(2013, 2, 2, 7, 0, 0) }, "Saturday is exempt.");

        PrintScenario(calculator, "Configured holiday", VehicleType.Car,
            new[] { new DateTime(2013, 1, 1, 7, 0, 0) }, "This date is in the exemption calendar.");

        PrintScenario(calculator, "July exemption", VehicleType.Car,
            new[] { new DateTime(2013, 7, 15, 7, 0, 0) }, "All of July is exempt under the exercise policy.");

        PrintScenario(calculator, "Free passage before charging starts", VehicleType.Car,
            new[] { At(5, 59), At(6), At(7) },
            "05:59 is ignored. The window starts at 06:00, so the total is 18 SEK.");
    }

    private static void RunCustomExample(TollCalculator calculator, string[] args)
    {
        if (args.Length < 3)
            throw new ArgumentException("Provide a vehicle, a date and at least one passage time.");

        // Accept category names only: Enum.TryParse alone also accepts arbitrary numbers.
        if (!Enum.GetNames<VehicleType>().Any(name => name.Equals(args[0], StringComparison.OrdinalIgnoreCase))
            || !Enum.TryParse(args[0], ignoreCase: true, out VehicleType vehicle)
            || vehicle == VehicleType.Unknown)
            throw new ArgumentException("Unknown vehicle category.");

        if (!DateOnly.TryParseExact(args[1], "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateOnly date))
            throw new ArgumentException("Use a date in yyyy-MM-dd format.");

        var passages = new List<DateTime>();
        foreach (string value in args.Skip(2))
        {
            if (!TimeOnly.TryParseExact(value, new[] { "HH:mm", "HH:mm:ss", "HH:mm:ss.FFFFFFF" },
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out TimeOnly time))
                throw new ArgumentException($"Invalid passage time '{value}'. Use HH:mm or HH:mm:ss, optionally with fractional seconds.");

            passages.Add(date.ToDateTime(time, DateTimeKind.Unspecified));
        }

        PrintScenario(calculator, "Your passages", vehicle, passages.ToArray(),
            "Standalone fees below are combined by the hourly rule before the daily cap.");
    }

    // An ordinary weekday covered by the default calendar. Edit times to experiment.
    private static DateTime At(int hour, int minute = 0) => new(2013, 2, 4, hour, minute, 0);

    private static void PrintScenario(TollCalculator calculator, string title,
        VehicleType vehicle, DateTime[] passages, string explanation)
    {
        // Validate and calculate first, so invalid input cannot produce a partial report.
        int totalFee = calculator.CalculateDailyFee(vehicle, passages);
        Console.WriteLine($"{title} ({vehicle})");
        Console.WriteLine("Passage                         Standalone fee");

        foreach (DateTime passage in passages.OrderBy(date => date))
        {
            string timestamp = passage.ToString("yyyy-MM-dd HH:mm:ss.FFFFFFF", CultureInfo.InvariantCulture);
            Console.WriteLine($"{timestamp,-30}  {calculator.GetPassageFee(vehicle, passage),2} SEK");
        }

        Console.WriteLine($"Daily total after hourly grouping and cap: {totalFee} SEK");
        Console.WriteLine(explanation);
        Console.WriteLine();
    }

    private static void PrintHelp()
    {
        Console.WriteLine("Run without arguments for ten explained examples.");
        Console.WriteLine("Custom input: <vehicle> <yyyy-MM-dd> <HH:mm[:ss]> ...");
        Console.WriteLine("Example: Car 2013-02-04 05:59 06:00 07:00");
        Console.WriteLine("Vehicles: Car, Other, Motorbike, Tractor, Emergency, Diplomat, Foreign, Military.");
        Console.WriteLine("The bundled exemption calendar covers 2013. All times are city-local.");
    }
}
