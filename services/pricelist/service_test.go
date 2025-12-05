package pricelist_test

import (
	"testing"
	"time"

	mock_pricelist "afry-toll-calculator/mocks/afry-toll-calculator/services/pricelist"
	"afry-toll-calculator/services/pricelist"
)

func TestNew(t *testing.T) {
	minutesFromMidnightToTime := func(minutesSinceMidnight int) time.Time {
		zeroTime, _ := time.Parse("15:04", "00:00")
		return zeroTime.Add(time.Minute * time.Duration(minutesSinceMidnight))
	}

	tests := []struct {
		name              string
		priceBlocksGetter *mock_pricelist.MockPriceBlockGetter
		mocks             func(getter *mock_pricelist.MockPriceBlockGetter)
		checkPrices       map[int]int
	}{
		{
			name: "new calls PriceBlockGetter.GetPriceBlocks and sets priceOfMinute",
			mocks: func(getter *mock_pricelist.MockPriceBlockGetter) {
				getter.EXPECT().GetPriceBlocks().Return([]pricelist.PriceBlock{
					{Start: 0, Price: 100},
					{Start: 60, Price: 200},
				})
			},
			checkPrices: map[int]int{0: 100, 10: 100, 59: 100, 60: 200, 88: 200},
		},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			priceBlocksGetter := mock_pricelist.NewMockPriceBlockGetter(t)
			tt.mocks(priceBlocksGetter)

			s := pricelist.New(priceBlocksGetter)
			for k, v := range tt.checkPrices {
				if s.GetPrice(minutesFromMidnightToTime(k)) != v {
					t.Errorf("price for %v minutes is %v, want %v", k, s.GetPrice(minutesFromMidnightToTime(k)), v)
				}
			}
		})
	}
}
