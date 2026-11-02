namespace TollFeeCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Test test = new Test();
            int fee = 0;

            Console.WriteLine("Hämtar avgift utifrån data ...");
            fee = test.GetFee();

            Console.WriteLine($"Total avgift: {fee} SEK");
            Console.WriteLine("Tryck på valfri tangent för att avsluta...");
            Console.ReadKey();
        }
    }
}
 