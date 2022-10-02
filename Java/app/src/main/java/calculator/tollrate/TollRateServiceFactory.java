package calculator.tollrate;

public class TollRateServiceFactory
{
    public static TollRateService getTollRateService(String dataPath){
       return new TollRateServiceImpl(dataPath);
    }

}
