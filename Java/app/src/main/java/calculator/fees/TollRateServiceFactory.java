package calculator.fees;

public class TollRateServiceFactory
{
    public static TollRateService getTollRateService(String dataPath){
       return new TollRateServiceImpl(dataPath);
    }

}
