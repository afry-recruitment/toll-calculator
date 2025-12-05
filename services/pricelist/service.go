package pricelist

import (
	"time"
)

type Service interface {
	GetPrice(entry time.Time) int
}

type svc struct {
	priceOfMinute map[int]int
}

// New initializes and returns a new Service implementation using the provided PriceBlockGetter.
func New(priceBlocksGetter PriceBlockGetter) Service {
	priceBlocks := priceBlocksGetter.GetPriceBlocks()

	s := svc{}
	s.priceOfMinute = s.createPriceBlockLookup(priceBlocks)

	return &s
}
