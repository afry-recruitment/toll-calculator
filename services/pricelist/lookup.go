package pricelist

import "sort"

func (s *svc) createPriceBlockLookup(priceBlocks []PriceBlock) map[int]int {
	sort.Slice(priceBlocks, func(i, j int) bool { // ensure blocks are sorted correctly
		return priceBlocks[i].Start < priceBlocks[j].Start
	})

	lookup := map[int]int{}
	pb := 0
	i := 0
	for pb < 1440 {
		// if there are no more blocks, fill remaining minutes with 0
		if i >= len(priceBlocks) {
			lookup[pb] = 0
			pb++
			continue
		}

		// fill gap before current block with 0
		if pb < priceBlocks[i].Start {
			lookup[pb] = 0
			pb++
			continue
		}

		// determine end of current block
		end := 1440
		if i < len(priceBlocks)-1 {
			end = priceBlocks[i+1].Start
		}

		// fill current block with its price
		if pb < end {
			lookup[pb] = priceBlocks[i].Price
			pb++
		} else {
			i++
		}
	}

	return lookup
}
