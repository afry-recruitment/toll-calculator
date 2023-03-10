using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TollCalculatorAfry;
using TollCalculatorAfry.Services;

namespace TollCalculatorAfryTest
{
    [TestClass]
    public class TollFeeServiceTest
    {
        [DataRow("2023/03/08T06:10:33", 8)]
        [DataRow("2023/03/04T06:30:33", 13)] 
        [DataRow("2023/03/05T15:10:33", 13)]
        [DataRow("2023/03/05T15:40:33", 18)]
        [DataRow("2023/03/05T19:10:33", 0)]
        [DataTestMethod]
        public void is_toll_free_test(string passage, int fee)
        {
            var passedDate = DateTime.Parse(passage);
            var res= TollService.GetFeeForTheTime(passedDate);
            Assert.IsTrue(res == fee);

        }


        [DataRow(new string[] { "2021-01-15T06:10:33" }, 8)]
        [DataRow(new string[] { "2021-01-15T06:20:33", "2021-01-15T07:10:33" }, 18)]
        [DataTestMethod]
        public void get_highest_fee_for_the_hour(string[] passage, int fee)
        {
            var dateArray = new DateTime[passage.Length];
            // Arrange
            for (int i = 0; i < passage.Length; i++)
            {
                dateArray[i] = DateTime.Parse(passage[i]);
            }
            // Act
            var tollFreeRes = TollService.GetTheHighest(dateArray);
            Console.WriteLine(tollFreeRes);

            // Assert
            Assert.IsTrue(tollFreeRes == fee);
        }
    }
}
