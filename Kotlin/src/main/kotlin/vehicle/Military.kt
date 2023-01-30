package vehicle

class Military: Vehicle {
    override fun getType(): VehicleType {
        return VehicleType.MILITARY
    }

    override val isTaxExempt: Boolean
        get() = true
}