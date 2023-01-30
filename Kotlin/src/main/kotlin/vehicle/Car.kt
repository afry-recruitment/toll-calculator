package vehicle

class Car: Vehicle {
    override fun getType(): VehicleType = VehicleType.Car
    override val isTaxExempt: Boolean = false
}
