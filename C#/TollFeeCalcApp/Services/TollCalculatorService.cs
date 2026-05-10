public class TollCalculatorService : ITollCalculatorService
{
    public bool IsTollFree(IVehicle vehicle)
    {
        /* 
            Gjorde såhär istället för attribut på varje vehicle.
            Tänkte lite som ett troligt scenario (Vad jag själv tror) att vi som har TollCalculator appen har det som config om ett visst vehicle är
            tollFree eller inte. Då kanske infon om själva typerna av vehicle (klasserna) kommer från ett externt system, 
            ungefär som att man importerar produktinfo från externt in i e-handelns PIM.
            
        */
        return vehicle?.Type switch
        {
            VehicleType.Tractor => true,
            VehicleType.Motorbike => true,
            VehicleType.Emergency => true,
            VehicleType.Diplomat => true,
            VehicleType.Foreign => true,
            VehicleType.Military => true,
            _ => false
        };
    }
}