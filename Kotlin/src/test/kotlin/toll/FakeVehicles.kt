package toll

import vehicle.Vehicle
import vehicle.VehicleType

object FakeVehicles {
    val taxedCar = object: Vehicle {
        override fun getType(): VehicleType {
            return VehicleType.Car
        }

        override val isTaxExempt: Boolean = false

    }

    val taxExemptCar = object: Vehicle {
        override fun getType(): VehicleType = VehicleType.Car

        override val isTaxExempt: Boolean = true
    }
}