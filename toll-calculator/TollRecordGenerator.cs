
public class TollRecordGenerator
{
    /// <summary>
    /// Test class to randomize entires for the TollProcessor
    /// </summary>
    public List<TollRecord> GenerateInstances(int amount)
    {
        int maxInstances = 50;
        List<TollRecord> tollRecord = new List<TollRecord>();

        Random random = new Random();

        for (int i = 0; i < amount; i++)
        {
            //Generate plate number
            string plateNumber = "ABC123";

            //Generate Type
            VehicleType type = (VehicleType)random.Next(0, Enum.GetValues(typeof(VehicleType)).Length);

            //Generate Dates
            int numDates = random.Next(1, maxInstances);
            DateTime[] randomDates = GenerateRandomDates(numDates);

            // Create a Vehicle instance
            Vehicle vehicle = new Vehicle(plateNumber, type);

            tollRecord.Add(new TollRecord(vehicle, randomDates));
        }

        return tollRecord;
    }

    DateTime[] GenerateRandomDates(int numDates)
    {
        Random random = new Random();
        DateTime[] dates = new DateTime[numDates];
        DateTime startOfDay = DateTime.MinValue;

        for (int i = 0; i < numDates; i++)
        {
            DateTime randomDateTime = startOfDay.AddHours(random.Next(1, 23));
            randomDateTime = randomDateTime.AddDays(random.Next(0, 26));
            randomDateTime = randomDateTime.AddMinutes(random.Next(1, 59));
            randomDateTime = randomDateTime.AddMonths(DateTime.Now.Month - 1);
            randomDateTime = randomDateTime.AddYears(DateTime.Now.Year-1);

            dates[i] = randomDateTime;
        }
        return dates;
    }

}
