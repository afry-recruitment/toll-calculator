import java.util.HashSet;
import java.util.Set;
public class VehicleChecker {


        public static final Set<Vehicles> TOLL_FREE_VEHICLES = new HashSet<>();

         static {
            TOLL_FREE_VEHICLES.add(Vehicles.MOTORBIKE);
            TOLL_FREE_VEHICLES.add(Vehicles.TRACTOR);
            TOLL_FREE_VEHICLES.add(Vehicles.EMERGENCY);
            TOLL_FREE_VEHICLES.add(Vehicles.DIPLOMAT);
            TOLL_FREE_VEHICLES.add(Vehicles.FOREIGN);
            TOLL_FREE_VEHICLES.add(Vehicles.MILITARY);
        }

        public static boolean isTollFree(Vehicles vehicle) {
            return TOLL_FREE_VEHICLES.contains(vehicle);
        }

}
