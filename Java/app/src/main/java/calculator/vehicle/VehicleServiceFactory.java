package calculator.vehicle;

public class VehicleServiceFactory
{
    public static VehicleService getVehicleService(String csvPath){
        return new VehicleServiceImpl(csvPath);
    }
}
