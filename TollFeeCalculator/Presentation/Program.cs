using System.Globalization;
using TollFeeCalculator.Application;
using TollFeeCalculator.Domain.Vehicle;

namespace TollFeeCalculator.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Toll Fee Calculator ===");
            Console.WriteLine();

            CultureInfo selectedCulture = GetLocaleFromUser();
            CultureInfo.CurrentCulture = selectedCulture;
            CultureInfo.CurrentUICulture = selectedCulture;
            Console.WriteLine($"Using locale: {selectedCulture.Name}");
            Console.WriteLine();

            IVehicle vehicle = GetVehicleFromUser();

            List<DateTime> dates = GetTollDatesFromUser();
            if (dates.Count == 0)
            {
                Console.WriteLine("No valid dates entered. Exiting.");
                return;
            }

            TollCalculator calculator = new();
            int totalFee = TollCalculator.GetFinalTollFee(vehicle, [.. dates]);
            Console.WriteLine();
            Console.WriteLine($"Total Toll Fee: {totalFee} " + CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol);
        }

        private static CultureInfo GetLocaleFromUser()
        {
            Console.Write("Enter locale/country code (e.g., en-US, sv-SE): ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("No locale entered. Defaulting to sv-SE.");
                input = "sv-SE";
            }

            try
            {
                return new CultureInfo(input);
            }
            catch (CultureNotFoundException)
            {
                Console.WriteLine("Invalid locale. Defaulting to sv-SE.");
                return new CultureInfo("sv-SE");
            }
        }

        private static IVehicle GetVehicleFromUser()
        {
            Console.WriteLine("Select Vehicle Type:");
            Console.WriteLine("1. Car");
            Console.WriteLine("2. Motorbike");
            Console.WriteLine("3. Tractor");
            Console.WriteLine("4. Emergency");
            Console.WriteLine("5. Diplomat");
            Console.WriteLine("6. Foreign");
            Console.WriteLine("7. Military");
            Console.Write("Enter option number: ");
            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    return new Car();
                case "2":
                    return new Motorbike();
                case "3":
                    return new Tractor();
                case "4":
                    return new Emergency();
                case "5":
                    return new Diplomat();
                case "6":
                    return new Foreign();
                case "7":
                    return new Military();
                default:
                    Console.WriteLine("Invalid option. Defaulting to Car.");
                    return new Car();
            }
        }

        private static List<DateTime> GetTollDatesFromUser()
        {
            List<DateTime> dates = new List<DateTime>();
            Console.WriteLine("Enter date/time entries for toll calculation.");
            Console.WriteLine("Use format: yyyy-MM-dd HH:mm (e.g., 2021-09-10 06:15).");
            Console.WriteLine("Type 'done' when finished.");

            while (true)
            {
                Console.Write("Enter date/time: ");
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input) || !string.IsNullOrEmpty(input) && input.Equals("done", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", CultureInfo.CurrentCulture,
                               DateTimeStyles.None, out DateTime dt))
                {
                    dates.Add(dt);
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                }
            }

            return dates;
        }
    }
}
