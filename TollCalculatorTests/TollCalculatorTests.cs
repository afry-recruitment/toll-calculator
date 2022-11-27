using System.ComponentModel.DataAnnotations;
using TollFeeCalculator.Models;
using TollFeeCalculator;
using TollFeeCalculator.Interfaces;

namespace TollCalculatorTests
{
    public class Tests
    {
        private readonly IVehicle _car = new Car();
        private readonly IVehicle _motorbike = new Motorbike();
        private readonly IVehicle _diplomat = new Diplomat();
        private readonly IVehicle _emergency = new Emergency();
        private readonly IVehicle _foreign = new Foreign();
        private readonly IVehicle _military = new Military();
        private readonly IVehicle _tractor = new Tractor();

        private readonly TollCalculator _tollCalculator = new TollCalculator();

        [Test]
        public void TollFreeVehicleTest()
        {
            Assert.IsFalse(_tollCalculator.IsTollFreeVehicle(_car));

            Assert.IsTrue(_tollCalculator.IsTollFreeVehicle(_motorbike));
            Assert.IsTrue(_tollCalculator.IsTollFreeVehicle(_diplomat));
            Assert.IsTrue(_tollCalculator.IsTollFreeVehicle(_emergency));
            Assert.IsTrue(_tollCalculator.IsTollFreeVehicle(_foreign));
            Assert.IsTrue(_tollCalculator.IsTollFreeVehicle(_military));
            Assert.IsTrue(_tollCalculator.IsTollFreeVehicle(_tractor));
        }

        [Test]
        public void TollFreeDatesTest()
        {
            Assert.IsTrue(_tollCalculator.IsTollFreeDate(new DateTime(2013, 01, 01)));
            Assert.IsTrue(_tollCalculator.IsTollFreeDate(new DateTime(2013, 07, 15)));
            Assert.IsFalse(_tollCalculator.IsTollFreeDate(new DateTime(2013, 08, 01)));
        }

        [Test]
        public void TollFreeSaturdaySundayTest()
        {
            Assert.IsTrue(_tollCalculator.IsTollFreeDate(new DateTime(2013, 01, 05))); //saturday
            Assert.IsTrue(_tollCalculator.IsTollFreeDate(new DateTime(2013, 05, 12))); //sunday
            Assert.IsFalse(_tollCalculator.IsTollFreeDate(new DateTime(2013, 10, 16))); //wednesday
        }


        [Test]
        public void GetTollFee_EightSEKTest()
        {
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 6, 15, 0), _car), Is.EqualTo(8));
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 8, 45, 0), _car), Is.EqualTo(8));
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 18, 15, 0), _car), Is.EqualTo(8));
        }

        [Test]
        public void GetTollFee_ThirteenSEKTest()
        {
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 6, 45, 0), _car), Is.EqualTo(13));
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 8, 15, 0), _car), Is.EqualTo(13));
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 15, 15, 0), _car), Is.EqualTo(13));
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 17, 15, 0), _car), Is.EqualTo(13));
        }

        [Test]
        public void GetTollFee_EighteenSEKTest()
        {
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 7, 15, 0), _car), Is.EqualTo(18));
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 15, 45, 0), _car), Is.EqualTo(18));
        }

        [Test]
        public void GetTollFee_ZeroSEKTest()
        {
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 5, 0, 0), _car), Is.EqualTo(0));
            Assert.That(_tollCalculator.GetTollFeeForPassage(new DateTime(2013, 01, 15, 22, 0, 0), _car), Is.EqualTo(0));
        }

        [Test]
        public void GetTollFee_OnePassage()
        {
            DateTime[] _passageTimes = new DateTime[]
            {
                new DateTime(2013, 01, 15, 6, 0, 0),
            };
            Assert.That(_tollCalculator.GetTollFee(_car, _passageTimes), Is.EqualTo(8));
        }

        [Test]
        public void GetTollFee_MultiplePassagesTest()
        {
            DateTime[] _passageTimes = new DateTime[]
            {
                new DateTime(2013, 01, 15, 6, 15, 0),
                new DateTime(2013, 01, 15, 12, 15, 0),
                new DateTime(2013, 01, 15, 18, 15, 0),
            };
            Assert.That(_tollCalculator.GetTollFee(_car, _passageTimes), Is.EqualTo(24));
        }

        [Test]
        public void GetTollFee_MultiplePassagesWithinHoursTest()
        {
            DateTime[] _passageTimes = new DateTime[]
            {
                new DateTime(2013, 01, 15, 6, 0, 0),
                new DateTime(2013, 01, 15, 6, 15, 0),
                new DateTime(2013, 01, 15, 6, 30, 0),
                new DateTime(2013, 01, 15, 6, 45, 0), 

                new DateTime(2013, 01, 15, 15, 5, 0),
                new DateTime(2013, 01, 15, 16, 5, 0), 

                new DateTime(2013, 01, 15, 17, 13, 0),
                new DateTime(2013, 01, 15, 17, 29, 0),
                new DateTime(2013, 01, 15, 18, 5, 0),
            };
            Assert.That(_tollCalculator.GetTollFee(_car, _passageTimes), Is.EqualTo(44));
        }

        [Test]
        public void GetTollFee_MaxFeeForOneDayTest()
        {
            DateTime[] _passageTimes = new DateTime[]
            {
                new DateTime(2013, 01, 15, 6, 0, 0), 
                new DateTime(2013, 01, 15, 7, 5, 0),
                new DateTime(2013, 01, 15, 8, 10, 0), 
                new DateTime(2013, 01, 15, 9, 15, 0), 
                new DateTime(2013, 01, 15, 15, 30, 0), 
            };
            Assert.That(_tollCalculator.GetTollFee(_car, _passageTimes), Is.EqualTo(60));
        }


        [Test]
        public void GetTollFee_NewTimeRangeSecondsTest()
        {
            DateTime[] _passageTimes = new DateTime[]
            {
                new DateTime(2013, 01, 15, 6, 0, 0),
                new DateTime(2013, 01, 15, 7, 0, 20),
            };
            Assert.That(_tollCalculator.GetTollFee(_car, _passageTimes), Is.EqualTo(26));
        }

        [Test]
        public void GetTollFee_NewTimeRangeMinuteTest()
        {
            DateTime[] _passageTimes = new DateTime[]
            {
                new DateTime(2013, 01, 15, 6, 0, 0),
                new DateTime(2013, 01, 15, 7, 1, 0),
            };
            Assert.That(_tollCalculator.GetTollFee(_car, _passageTimes), Is.EqualTo(26));
        }

        [Test]
        public void GetTollFee_PassageAtMidnightTest()
        {
            DateTime[] _passageTimes = new DateTime[]
            {
                new DateTime(2013, 01, 15, 0, 0, 0),
            };
            Assert.That(_tollCalculator.GetTollFee(_car, _passageTimes), Is.EqualTo(0));
        }

        [Test]
        public void GetTollFee_VehicleIsNull()
        {
            IVehicle vehicle = null;
            DateTime[] _passageTimes = new DateTime[]
            {
                new DateTime(2013, 01, 15, 0, 0, 0),
            };
            Assert.That(_tollCalculator.GetTollFee(vehicle, _passageTimes), Is.EqualTo(0));
        }


        [Test]
        public void GetTollFee_PassingTimesIsNull()
        {
            DateTime[] _passageTimes = null;
            Assert.That(_tollCalculator.GetTollFee(_car, _passageTimes), Is.EqualTo(0));
        }


        [Test]
        public void GetTollFee_NoPassingTimes()
        {
            DateTime[] _passageTimes = new DateTime[] { };
            Assert.That(_tollCalculator.GetTollFee(_car, _passageTimes), Is.EqualTo(0));
        }

    }
}