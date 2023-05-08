namespace TollFeeCalculator
{
  class Program
  {
    static void Main(string[] args)
    {
      var dates = new DateTime[] { new DateTime(2003, 2, 12, 6, 23, 0) };
      Calculator calculator = new Calculator(new FeeService(), new TollFreeDateService());
      calculator.TollFee(3, dates);
    }
  }
}