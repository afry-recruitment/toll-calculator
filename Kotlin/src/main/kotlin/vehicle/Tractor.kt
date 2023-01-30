package vehicle

class Tractor: Vehicle {
    override fun getType(): VehicleType {
        return VehicleType.TRACTOR
    }

    override val isTaxExempt: Boolean
        get() = true
}