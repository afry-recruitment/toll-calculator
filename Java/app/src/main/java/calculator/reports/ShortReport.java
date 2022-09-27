package calculator.reports;

import calculator.fees.Result;

public class ShortReport implements Report
{
    @Override
    public String makeReport(Result result)
    {
        return "Toll fee report" + System.lineSeparator() + "Vehicle: " + result.getVehicleId() +
               System.lineSeparator() + "Type of vehicle: " + result.getVehicleType() +
               System.lineSeparator() + "Start of fee period: " + result.getStartOfPeriod() +
               System.lineSeparator() + "End of fee period: " + result.getEndOfPeriod() +
               System.lineSeparator() + "Actual cost: " + result.getActualFee() + System.lineSeparator() +
               System.lineSeparator();
    }
}
