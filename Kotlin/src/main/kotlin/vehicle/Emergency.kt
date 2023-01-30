package vehicle

class Emergency: Vehicle {
    override fun getType(): VehicleType {
        return VehicleType.EMERGENCY
    }

    override val isTaxExempt: Boolean
        get() = true
}