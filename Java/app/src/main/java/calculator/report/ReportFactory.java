package calculator.report;

import calculator.report.reports.FullReport;
import calculator.report.reports.ShortReport;

public class ReportFactory
{
    public final static String SHORT_REPORT = "SHORT";
    public final static String FULL_REPORT = "FULL";

    private ReportFactory()
    {
    }

    public static Report getReport(String report)
    {
        if (report.equals(SHORT_REPORT)) return new ShortReport();
        if (report.equals(FULL_REPORT)) return new FullReport();
        throw new IllegalArgumentException("No report with that name exists. ");
    }

}
