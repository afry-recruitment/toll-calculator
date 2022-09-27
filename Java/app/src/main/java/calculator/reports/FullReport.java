package calculator.reports;

import calculator.fees.Result;

public class FullReport implements Report
{

    @Override
    public String makeReport(Result result)
    {
        return "Toll fee report" + System.lineSeparator() + "Vehicle: " + result.getVehicleId() +
               System.lineSeparator() + "Type of vehicle: " + result.getVehicleType() +
               System.lineSeparator() + "Start of fee period: " + result.getStartOfPeriod() +
               System.lineSeparator() + "End of fee period: " + result.getEndOfPeriod() +
               System.lineSeparator() + "Number of passes: " + result.numberOfPasses() +
               System.lineSeparator() + "Total cost: " + result.getTotalCost() +
               System.lineSeparator() + "Total cost reduction due to interval price roofs: " +
               (result.getTotalCost() - result.getActualFee()) + System.lineSeparator() + "Actual cost: " +
               result.getActualFee() + System.lineSeparator() + System.lineSeparator() + "This report" +
               " was generated: " + result.getCreationTime() + System.lineSeparator() +
               System.lineSeparator();
    }

}
