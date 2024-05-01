namespace Tollculator.Generators;

public static class SampleDataParser
{
    public static IDictionary<string, IList<TimeOnly>> ParseLogFile(string path)
    {
        var inputList = ReadFromFile(path);

        var dictionary = new Dictionary<string, IList<TimeOnly>>();

        foreach (var input in inputList)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }
            var registrationNumber = input.Split(" ")[1];

            var time = TimeOnly.Parse(input.Split(" ")[0]);

            if (dictionary.TryGetValue(registrationNumber, out IList<TimeOnly>? value))
            {
                value.Add(time);
            }
            else
            {
                dictionary.Add(registrationNumber, [time]);
            }
        }

        return dictionary;
    }

    private static string[] ReadFromFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                using StreamReader reader = new(filePath);
                return reader.ReadToEnd().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            }
            else
            {
                Console.WriteLine($"File '{filePath}' does not exist.");
                return [];
            }
        }
        catch
        {
            Console.WriteLine("An error occured.");
            return [];
        }
    }
}
