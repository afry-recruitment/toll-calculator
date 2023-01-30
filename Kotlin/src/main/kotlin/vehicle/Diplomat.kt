package vehicle

class Diplomat: Vehicle {
    override fun getType(): VehicleType = VehicleType.DIPLOMAT
    override val isTaxExempt: Boolean = true
}