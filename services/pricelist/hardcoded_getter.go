package pricelist

// Ensure conformance to the interface
var _ PriceBlockGetter = (*HardcodedPriceBlocksGetter)(nil)

// HardcodedPriceBlocksGetter provides a static representation of the PriceBlockGetter interface with predefined price blocks.
type HardcodedPriceBlocksGetter struct{}

func (s *HardcodedPriceBlocksGetter) GetPriceBlocks() []PriceBlock {
	return []PriceBlock{
		{Start: mustMinutes("00:00"), Price: 0},
		{Start: mustMinutes("06:00"), Price: 8},
		{Start: mustMinutes("06:30"), Price: 13},
		{Start: mustMinutes("07:00"), Price: 18},
		{Start: mustMinutes("08:00"), Price: 13},
		{Start: mustMinutes("08:30"), Price: 8},
		{Start: mustMinutes("15:00"), Price: 13},
		{Start: mustMinutes("15:30"), Price: 18},
		{Start: mustMinutes("17:00"), Price: 13},
		{Start: mustMinutes("18:00"), Price: 8},
		{Start: mustMinutes("18:30"), Price: 0},
	}
}
