using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TollCalculatorAfry;

namespace TollCalculatorAfryTest
{
    [TestClass]
    public class TollCalculatorTest
    {
        TollCalculator tollCalculator = new TollCalculator();

        [TestMethod]
        public void get_toll_fee_test()
        {
            var res= tollCalculator.GetTollFee(new Car(), new[] { new DateTime(2023,1,15,6, 0, 0) });
            Assert.IsTrue(res == 0);

        }
    }
}
