using Xunit;

namespace toll_calculator.tests;


public class AcceptanceTests
{
    //Fees will differ between 8 SEK and 18 SEK, depending on the time of day

    [Fact]
    public void Rush_hour_traffic_will_render_highest_fee()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void The_maximum_fee_for_one_day_is_60_SEK()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void A_vehicle_should_only_be_charged_once_an_hour()
    {
        //In the case of multiple fees in the same hour period, the highest one applies.
        throw new NotImplementedException();
    }

    [Fact]
    public void Some_vehicle_types_are_fee_free()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Weekends_and_holidays_are_fee_free()
    {
        throw new NotImplementedException();
    }
}
