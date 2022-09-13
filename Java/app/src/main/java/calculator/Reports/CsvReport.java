package calculator.Reports;

public class CsvReport
{
    private String regId;
    private int noPasses;
    private int fee;

    public CsvReport(String regId, int noPasses, int fee)
    {
        this.regId = regId;
        this.noPasses = noPasses;
        this.fee = fee;
    }

    @Override
    public String toString()
    {
        return regId + "," + noPasses + "," + fee + System.lineSeparator();
    }
}
