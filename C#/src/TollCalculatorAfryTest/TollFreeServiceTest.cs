using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TollCalculatorAfry;
using TollCalculatorAfry.Services;

namespace TollCalculatorAfryTest
{
    [TestClass]
    public class TollFreeServiceTest
    {
        [DataRow("2023/03/08", false)]
        [DataRow("2023/03/04", true)] // saturday
        [DataRow("2023/03/05", true)] // sunday
        [DataTestMethod]
        public void is_toll_free_test(string passage, bool isFree)
        {
            var passedDate = DateTime.Parse(passage);
            var res= TollFreeService.IsTollFree(passedDate);
            Assert.IsTrue(res == isFree);

        }

        [DataRow(typeof(Car), false)]
        [DataRow(typeof(Motorbike), true)]
        [DataTestMethod]
        public void is_toll_free_vehicle_test(Type vehicleObj, bool isFree)
        {
            var vehicle = (IVehicle)Activator.CreateInstance(vehicleObj);

            var res = TollFreeService.IsTollFreeVehicle(vehicle);
            Assert.IsTrue(res == isFree);

        }
    }
}
