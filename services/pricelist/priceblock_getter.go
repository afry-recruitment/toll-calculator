package pricelist

type PriceBlock struct {
	Start int
	Price int
}

// PriceBlockGetter defines an interface for retrieving a list of price blocks to allow easy transition
// to a configurable, storage-based approach by replacing HardcodedGetter to any memory implementation.
type PriceBlockGetter interface {
	GetPriceBlocks() []PriceBlock
}
