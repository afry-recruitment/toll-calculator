package toll

enum class TollPrice(val price: Int) {
    Free(price = 0),
    Low(price = 8),
    Medium(price = 13),
    High(price = 18),
}
