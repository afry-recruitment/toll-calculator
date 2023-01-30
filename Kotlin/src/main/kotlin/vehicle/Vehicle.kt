package vehicle

interface Vehicle {
    fun getType(): VehicleType
    val isTaxExempt: Boolean
}
