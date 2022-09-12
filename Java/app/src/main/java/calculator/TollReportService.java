package calculator;

public class TollReportService
{
    private TollReportService()
    {

    }

    public static String buildCsvReportLine(String regId, int noPasses, int fee)
    {
        return regId + "," + noPasses + "," + fee + System.lineSeparator();
    }
}
