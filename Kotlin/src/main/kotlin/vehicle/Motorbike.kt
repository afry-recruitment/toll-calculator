package vehicle

class Motorbike: Vehicle {
    override fun getType(): VehicleType = VehicleType.Motorbike
    override val isTaxExempt: Boolean
        get() = true
}
