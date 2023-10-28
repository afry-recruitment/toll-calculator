/*
 * Author: Marcus Klein - kleinmarcus@live.se
 * Date: 28/10/2023
 * 
 */
class Program
{
    private static void Main(string[] args)
    {
        //Instantiate classes
        TollProcessor tollProcessor = new TollProcessor();
        TollRecordGenerator generator = new TollRecordGenerator();

        //Create examples
        var tollRecords = generator.GenerateInstances(20);

        for (int i = 0; i < tollRecords.Count; i++)
        {
            Console.WriteLine("New vehicle");

            TollRecord record = tollRecords[i];
            int cost = tollProcessor.ProcessToll(record);

            Console.WriteLine(
                $"A {record.vehicle.vehicleType} with plate number:" +
                $" {record.vehicle.licensePlate} on" +
                $" {record.instances.Length} occasions between" +
                $" {record.instances.Min()} and {record.instances.Max()}" +
                $" recieved the following toll:" +
                $" {cost} SEK \n" +
                "----------------- \n"
                );
        }
    }
}

