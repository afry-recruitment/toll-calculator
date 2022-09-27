package calculator.vehicle;

import java.time.ZonedDateTime;
import java.util.List;

public record VehiclePassageHistory(String vehicleId, VehicleType vehicleType, List<ZonedDateTime> passes)
{

}
