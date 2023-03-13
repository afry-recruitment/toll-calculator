using System;
using TollCalculator.Interface;
using TollCalculator.Repos;
using Xunit;


namespace XUnitTestTollCalculator
{
    public class XUnitTestTollCalculator
    {

        [Fact]
        public void Test_all_free_days_2023_no_toll()
        {
            // test all days without toll
            //5 - 6 januari
            Assert.False(PassingDate.IsDateTollDate(new(2023, 1, 5)));
            Assert.False(PassingDate.IsDateTollDate(new(2023, 1, 6)));

            //6 - 7 april
            Assert.False(PassingDate.IsDateTollDate(new(2023, 4, 6)));
            Assert.False(PassingDate.IsDateTollDate(new(2023, 4, 7)));

            //10 april
            Assert.False(PassingDate.IsDateTollDate(new(2023, 4, 10)));

            //1 maj
            Assert.False(PassingDate.IsDateTollDate(new(2023, 5, 1)));

            //17 - 18 maj
            Assert.False(PassingDate.IsDateTollDate(new(2023, 5, 17)));
            Assert.False(PassingDate.IsDateTollDate(new(2023, 5, 18)));

            //5 - 6 juni
            Assert.False(PassingDate.IsDateTollDate(new(2023, 6, 5)));
            Assert.False(PassingDate.IsDateTollDate(new(2023, 6, 6)));

            //23 juni
            Assert.False(PassingDate.IsDateTollDate(new(2023, 6, 23)));
            // 22 juni normal day
            Assert.True(PassingDate.IsDateTollDate(new(2023, 6, 22)));

            //3 november
            Assert.False(PassingDate.IsDateTollDate(new(2023, 11, 3)));

            //25 - 26 december
            Assert.False(PassingDate.IsDateTollDate(new(2023, 12, 25)));
            Assert.False(PassingDate.IsDateTollDate(new(2023, 12, 26)));
        }


        [Fact]
        public void Test_saturday_is_not_tollDay()
        {
            Assert.False(PassingDate.IsDateTollDate(new(2023, 3, 18)));
        }

        [Fact]
        public void Test_sunday_not_tollDay()
        {
            Assert.False(PassingDate.IsDateTollDate(new(2023, 3, 19)));
        }

        // Not as in Gothenburg
        //[Fact]
        //public void Test_juli_not_toll_day()
        //{
        //    Assert.False(PassingDate.IsDateTollDate(new(2023, 7, 1)));
        //}

        [Fact]
        public void Test_normal_day_is_toll_day()
        {
            Assert.True(PassingDate.IsDateTollDate(new(2023, 3, 17)));
        }


        [Fact]
        public void Test_two_passing_within_60_min()
        {
            IVehicle vehicle = new Car();
            DateTime[] passingDates = { new DateTime(2023, 3, 17), new DateTime(2023, 3, 17) };
            passingDates[0] = passingDates[0].AddHours(6).AddMinutes(6);
            passingDates[1] = passingDates[1].AddHours(6).AddMinutes(50);
            int totalCost = TollCalculator.TollCalculator.GetTollFee(vehicle, passingDates);
            Assert.Equal(13, totalCost);
        }

        [Fact]
        public void Test_two_passing_over_60_min()
        {
            IVehicle vehicle = new Car();
            DateTime[] passingDates = { new DateTime(2023, 3, 17), new DateTime(2023, 3, 17) };
            passingDates[0] = passingDates[0].AddHours(6).AddMinutes(6);
            passingDates[1] = passingDates[1].AddHours(7).AddMinutes(50);
            int totalCost = TollCalculator.TollCalculator.GetTollFee(vehicle, passingDates);
            Assert.Equal(18 + 8, totalCost);
        }

        [Fact]
        public void Test_max_60_per_day()
        {
            IVehicle vehicle = new Car();
            DateTime[] passingDates = { new DateTime(2023, 3, 17), new DateTime(2023, 3, 17), new DateTime(2023, 3, 17), new DateTime(2023, 3, 17) , new DateTime(2023, 3, 17) };
            passingDates[0] = passingDates[0].AddHours(6).AddMinutes(0);
            passingDates[1] = passingDates[1].AddHours(7).AddMinutes(1);
            passingDates[2] = passingDates[2].AddHours(8).AddMinutes(2);
            passingDates[3] = passingDates[3].AddHours(12).AddMinutes(0);
            passingDates[4] = passingDates[4].AddHours(17).AddMinutes(0);

            int totalCost = TollCalculator.TollCalculator.GetTollFee(vehicle, passingDates);
            Assert.Equal(60, totalCost);
        }

        [Fact]
        public void Test_max_60_two_days()
        {
            IVehicle vehicle = new Car();
            DateTime[] passingDates = { new DateTime(2023, 3, 17), 
                                        new DateTime(2023, 3, 17), 
                                        new DateTime(2023, 3, 17), 
                                        new DateTime(2023, 3, 17), 
                                        new DateTime(2023, 3, 17),
                                        new DateTime(2023, 3, 16),
                                        new DateTime(2023, 3, 16),
                                        new DateTime(2023, 3, 16),
                                        new DateTime(2023, 3, 16),
                                        new DateTime(2023, 3, 16),
            };

            passingDates[0] = passingDates[0].AddHours(6).AddMinutes(0);
            passingDates[1] = passingDates[1].AddHours(7).AddMinutes(1);
            passingDates[2] = passingDates[2].AddHours(8).AddMinutes(2);
            passingDates[3] = passingDates[3].AddHours(12).AddMinutes(0);
            passingDates[4] = passingDates[4].AddHours(17).AddMinutes(0);
            passingDates[5] = passingDates[5].AddHours(6).AddMinutes(0);
            passingDates[6] = passingDates[6].AddHours(7).AddMinutes(1);
            passingDates[7] = passingDates[7].AddHours(8).AddMinutes(2);
            passingDates[8] = passingDates[8].AddHours(12).AddMinutes(0);
            passingDates[9] = passingDates[9].AddHours(17).AddMinutes(0);

            int totalCost = TollCalculator.TollCalculator.GetTollFee(vehicle, passingDates);
            Assert.Equal(120, totalCost);
        }

        //06:00–06:29	8 kr
        [Fact]
        public void Test_time_and_cost_should_be_8kr()
        {
            DateTime dt = new(2023, 3, 17); // normal day
            dt = dt.AddHours(6).AddMinutes(5);
            Assert.Equal(8, PaymentInterval.GetCost(dt));

            DateTime dt2 = new(2023, 3, 17); // normal day
            dt2 = dt2.AddHours(6).AddMinutes(29);
            Assert.Equal(8, PaymentInterval.GetCost(dt2));
        }

        //06:30–06:59	13 kr
        [Fact]
        public void Test_time_and_cost_should_be_16kr()
        {
            DateTime dt = new(2023, 3, 17); // normal day
            dt = dt.AddHours(6).AddMinutes(30);
            Assert.Equal(13, PaymentInterval.GetCost(dt));

            DateTime dt2 = new(2023, 3, 17); // normal day
            dt2 = dt2.AddHours(6).AddMinutes(59);
            Assert.Equal(13, PaymentInterval.GetCost(dt2));

        }
        //08:30–14:59	8 kr

        [Fact]
        public void Test_time_and_cost_should_be_8kr_part2()

        {
            DateTime dt = new(2023, 3, 17); // normal day
            dt = dt.AddHours(8).AddMinutes(30);
            Assert.Equal(8, PaymentInterval.GetCost(dt))
                ;
            DateTime dt2 = new(2023, 3, 17); // normal day
            dt2 = dt2.AddHours(14).AddMinutes(59);
            Assert.Equal(8, PaymentInterval.GetCost(dt2));
        }

        //18:00–18:29	8 kr
        [Fact]
        public void TestTimeAndCost4()
        {
            DateTime dt = new(2023, 3, 17); // normal day
            dt = dt.AddHours(18).AddMinutes(15);
            int expektedResult = 8;
            int calculatedResult = PaymentInterval.GetCost(dt);
            Assert.Equal(expektedResult, calculatedResult);
        }

        //18:30–05:59	0 kr
        [Fact]
        public void TestTimeAndCost5()
        {
            DateTime dt = new(2023, 3, 17); // normal day
            dt = dt.AddHours(22).AddMinutes(15);
            int expektedResult = 0;
            int calculatedResult = PaymentInterval.GetCost(dt);
            Assert.Equal(expektedResult, calculatedResult);
        }

        [Fact]
        public void Test_diplomat_is_tollFree()
        {
            Diplomat d = new();
            bool expektedResult = true;
            bool calculatedResult = d.IsTollFree();
            Assert.Equal(expektedResult, calculatedResult);
        }

        [Fact]
        public void Test_car_not_toll_free()
        {
            Car d = new();
            bool expektedResult = false;
            bool calculatedResult = d.IsTollFree();
            Assert.Equal(expektedResult, calculatedResult);
        }

    }
}