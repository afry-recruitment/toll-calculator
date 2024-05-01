using Tollculator.VehicleRegister;

namespace Tollculator.Generators;

public static class SampleDataGenerator
{
     /// <summary>
     /// Generate a file that represents a logg of all the cars that passed the toll and what time they passed.
     /// </summary>
     /// <param name="path">The path of the file.</param>
    public static void GenerateLogFileForDay(string path)
    {
        var list = new List<string>();
        var random = new Random();

        foreach (var vehicle in Register.Vehicles)
        {
            var iterations = random.Next(10);
            for (int i = 0; i < iterations; i++)
            {
                var randomTime = GenerateRandomTime(random);
                list.Add($"{randomTime} {vehicle.Key}");
            }
        }

        list = [.. list.Order()];

        WriteToFile(path, [.. list]);
    }

    private static void WriteToFile(string filePath, string[] lines)
    {
        try
        {
            using StreamWriter writer = new(filePath);

            foreach (string line in lines)
            {
                writer.WriteLine(line);
            }
        }
        catch
        {
            Console.WriteLine("An error occured.");
        }
    }

    private static TimeOnly GenerateRandomTime(Random random)
    {
        var hour = random.Next(24);
        var minute = random.Next(60);

        return new TimeOnly(hour, minute);
    }
}
