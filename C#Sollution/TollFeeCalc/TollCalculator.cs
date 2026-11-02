namespace TollFeeCalculator
{
    public class TollCalculator
    {
        //Cacha så att vi bara behöver läsa in en gång, om vi skulle läsa in data för tusentals passeringar i taget etc. 
        private static readonly List<string> TollFeesTableList = GetTollFeesTableList();
        private static readonly List<string> TollFreeVehiclesList = GetTollFreeVehiclesList();

        //Denna läser vi in vart efter, vi kanske inte vet om vi får data från 2 år i samma körning
        //T.ex. 2026-12-31 + 2027-01-01
        private static readonly Dictionary<int, List<string>> TollFreeDatesByYear = new();

        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            //Total avgift för alla data/dagar som skickas in. 
            int totalFee = 0;

            //Snabbkoll så att vehicle inte är null och att dates finns och ej tomt. 
            if (vehicle == null)
            {
                throw new ArgumentNullException(nameof(vehicle));
            }
            if (dates == null || dates.Length == 0)
            {
                return 0;
            }

            //Dictionary med ett element per dag, med den dagens datum som nyckel. Values är DateTime för alla passeringar den dagen. 
            Dictionary<DateTime, List<DateTime>> datesByDay = new Dictionary<DateTime, List<DateTime>>();

            //Loopa igenom alla passeringar och bygg dict 
            foreach (DateTime date in dates)
            {
                DateTime day = date.Date;

                //Om dag redans finns i dict, lägg passeringen på den dagen. 
                if (!datesByDay.ContainsKey(day))
                {
                    datesByDay[day] = new List<DateTime>();
                }

                //Om dagen ej finns, Lägg till nytt element med den passeringen. 
                datesByDay[day].Add(date);
            }

            //Loopa igenom alla passeringar per dag. 
            foreach (var day in datesByDay)
            {
                //Lista om dom i ordning ifall dom ej las till i ordning
                List<DateTime> sortedDates = day.Value.OrderBy(d => d).ToList();

                //Börja med första i Listan
                DateTime intervalStart = sortedDates[0];

                int highestFee = GetTollFee(intervalStart, vehicle);
                int dailyFee = 0;

                //Loopa igenbom alla passeringar på den dagen, skippa första så att vi inte räknar den två gånger. 
                foreach (DateTime date in sortedDates.Skip(1))
                {
                    int nextFee = GetTollFee(date, vehicle);

                    //Hur lång tid sedan passeringen innan?
                    double minutes = (date - intervalStart).TotalMinutes;

                    //Känner mig snäll och räknar 06;45 och 07;45 som samma timme == bara den dyraste passeringen räknas.
                    if (minutes <= 60)
                    {
                        highestFee = Math.Max(highestFee, nextFee);
                    }
                    else
                    {
                        dailyFee += highestFee;

                        intervalStart = date;
                        highestFee = nextFee;
                    }
                }

                dailyFee += highestFee;

                //max 60 kr per dag
                totalFee += Math.Min(dailyFee, 60);
            }

            return totalFee;

        }

        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            foreach (string line in TollFeesTableList)
            {
                //Kolla om tom rad
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                string[] parts = line.Split(',');
                if (parts.Length != 2)
                {
                    throw new FormatException($"Invalid toll fee configuration: {line}");
                }

                string[] times = parts[0].Split('-');
                if (times.Length != 2)
                {
                    throw new FormatException($"Invalid time interval: {line}");
                }

                if (!TimeSpan.TryParse(parts[0].Split('-')[0].Trim(), out TimeSpan startTime) ||
                    !TimeSpan.TryParse(parts[0].Split('-')[1].Trim(), out TimeSpan endTime) ||
                    !int.TryParse(parts[1].Trim(), out int fee))
                {
                    throw new FormatException($"Invalid toll fee configuration: {line}");
                }

                TimeSpan currentMinute = new TimeSpan(date.Hour, date.Minute, 0);

                if (currentMinute >= startTime && currentMinute <= endTime)
                {
                    return fee;
                }
                   

            }

            return 0;
        }

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            /*Ersätter detta med att vi läser externt vilka fordon som är avgiftsfria. I detta fall från en textfil, så slipper man
            Korrigera koden varje gång man ska ändra vilka som är avgiftsfria. På detta sätt kan även ansvariga som ej är programmera korrigera detta.*/

            if (vehicle == null)
            {
                return false;
            }

            VehicleType vehicleType = vehicle.Type;

            return TollFreeVehiclesList.Select(v => v.Trim()).Any(v => Enum.TryParse<VehicleType>(v, true, out var type) && type == vehicle.Type);
        }


        //Flyttat ut till extern configuration där data är taget från stockholm 2026 som exempel: https://www.transportstyrelsen.se/trangselskattstockholm  
        private bool IsTollFreeDate(DateTime date)  
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;


            var tollFreeDates = GetTollFreeDatesForYear(date.Year);
            string dateToCheck = date.ToString("yyyy-MM-dd");

            return tollFreeDates.Any(line => line.Trim() == dateToCheck);         
        }

        private static List<string> GetTollFeesTableList()
        {
            List<string> ret = new List<string>();

            string filePath = Path.Combine(AppContext.BaseDirectory, "Config", "TollFeesTable.txt");

            if (!File.Exists(filePath))
            {
                throw new Exception("Error: Missing configuration file for toll fees. TollFeesTable.txt");
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                ret.Add(line);
            }

            return ret;
        }

        private static List<string> GetTollFreeVehiclesList()
        {
            List<string> ret = new List<string>();

            string filePath = Path.Combine(AppContext.BaseDirectory, "Config", "TollFreeVehicles.txt");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Toll-free vehicles configuration is missing.", filePath);
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                ret.Add(line);
            }

            return ret;
        }

        private static List<string> GetTollFreeDatesForYear(int year)
        {
            //Kolla om året vi vill ha finns i dict redan
            if (!TollFreeDatesByYear.TryGetValue(year, out var dates))
            {
                //Om det inte finns i dict redan så hämta. 
                dates = GetTollFreeDatesList(year);

                //spara den i dictionaryn så vi slipper läsa in den fler gånger.
                TollFreeDatesByYear[year] = dates;
            }

            return dates;
        }
        private static List<string> GetTollFreeDatesList(int year)
        {
            List<string> ret = new List<string>();

            string filePath = Path.Combine(AppContext.BaseDirectory, "Config", "TollFreeDates", $"{year}.txt");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Toll-free dates configuration for {year} is missing.", filePath);
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                ret.Add(line);
            }

            return ret;
        }
    }
}
