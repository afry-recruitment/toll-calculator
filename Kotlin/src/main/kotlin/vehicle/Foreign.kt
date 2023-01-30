package vehicle

class Foreign: Vehicle {
    override fun getType(): VehicleType {
        return VehicleType.FOREIGN
    }

    override val isTaxExempt: Boolean
        get() = true
}