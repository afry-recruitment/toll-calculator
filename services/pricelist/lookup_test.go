package pricelist

import (
	"testing"
	"time"
)

func Test_svc_createPriceBlockLookup(t *testing.T) {
	checkRange := func(in map[int]int, start, end, expectedPrice int) bool {
		for i := start; i < end; i++ {
			if in[i] != expectedPrice {
				return false
			}
		}
		return true
	}

	tests := []struct {
		name        string
		priceBlocks []PriceBlock
		wantFn      func(in map[int]int) bool
	}{
		{
			name: "should return lookup with prices",
			priceBlocks: []PriceBlock{
				{Start: mustMinutes("00:00"), Price: 0},
				{Start: mustMinutes("00:10"), Price: 20},
				{Start: mustMinutes("01:00"), Price: 30},
				{Start: mustMinutes("01:25"), Price: 10},
			},
			wantFn: func(in map[int]int) bool {
				return len(in) == 1440 &&
					checkRange(in, 0, 10, 0) &&
					checkRange(in, 10, 60, 20) &&
					checkRange(in, 60, 85, 30) &&
					checkRange(in, 85, 1440, 10)
			},
		},
		{
			name: "should return lookup with 1440 items",
			priceBlocks: []PriceBlock{
				{Start: mustMinutes("00:00"), Price: 15},
			},
			wantFn: func(in map[int]int) bool {

				return len(in) == 1440 &&
					checkRange(in, 0, 1440, 15)
			},
		},
		{
			name: "should fill price = 0 until first block",
			priceBlocks: []PriceBlock{
				{Start: mustMinutes("00:10"), Price: 15},
			},
			wantFn: func(in map[int]int) bool {
				return len(in) == 1440 &&
					checkRange(in, 0, 10, 0) &&
					checkRange(in, 10, 1440, 15)
			},
		},
		{
			name:        "no blocks should return price = 0 for all minutes",
			priceBlocks: []PriceBlock{},
			wantFn: func(in map[int]int) bool {
				return len(in) == 1440 &&
					checkRange(in, 0, 1440, 0)
			},
		},
		{
			name: "duplicate price block does not affect result",
			priceBlocks: []PriceBlock{
				{Start: mustMinutes("00:00"), Price: 15},
				{Start: mustMinutes("00:00"), Price: 15},
				{Start: mustMinutes("00:10"), Price: 25},
				{Start: mustMinutes("00:10"), Price: 25},
			},
			wantFn: func(in map[int]int) bool {
				return len(in) == 1440 &&
					checkRange(in, 0, 10, 15) &&
					checkRange(in, 10, 1440, 25)
			},
		},
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			s := &svc{
				priceOfMinute: map[int]int{},
			}
			if got := s.createPriceBlockLookup(tt.priceBlocks); !tt.wantFn(got) {
				t.Errorf("createPriceBlockLookup() = does not pass validation fn\ndata: %v", got)
			}
		})
	}
}

func BenchmarkCreatePriceBlockLookup_Allocs(b *testing.B) {
	svc := &svc{}
	priceBlocks := []PriceBlock{
		{Start: 0, Price: 0},
		{Start: 10, Price: 20},
		{Start: 60, Price: 30},
		{Start: 85, Price: 0},
	}

	b.ReportAllocs()
	b.ResetTimer()

	for i := 0; i < b.N; i++ {
		_ = svc.createPriceBlockLookup(priceBlocks)
	}
}

func BenchmarkCreatePriceBlockLookup(b *testing.B) {
	svc := &svc{}

	b.Run("empty", func(b *testing.B) {
		priceBlocks := []PriceBlock{}

		b.ResetTimer()
		for i := 0; i < b.N; i++ {
			_ = svc.createPriceBlockLookup(priceBlocks)
		}
	})

	b.Run("single block", func(b *testing.B) {
		priceBlocks := []PriceBlock{
			{Start: 0, Price: 100},
		}

		b.ResetTimer()
		for i := 0; i < b.N; i++ {
			_ = svc.createPriceBlockLookup(priceBlocks)
		}
	})

	b.Run("few blocks", func(b *testing.B) {
		priceBlocks := []PriceBlock{
			{Start: 0, Price: 0},
			{Start: 10, Price: 20},
			{Start: 60, Price: 30},
			{Start: 85, Price: 0},
		}

		b.ResetTimer()
		for i := 0; i < b.N; i++ {
			_ = svc.createPriceBlockLookup(priceBlocks)
		}
	})

	b.Run("many blocks", func(b *testing.B) {
		// One block every 10 minutes
		priceBlocks := make([]PriceBlock, 144)
		for i := 0; i < 144; i++ {
			priceBlocks[i] = PriceBlock{
				Start: i * 10,
				Price: i * 5,
			}
		}

		b.ResetTimer()
		for i := 0; i < b.N; i++ {
			_ = svc.createPriceBlockLookup(priceBlocks)
		}
	})

	b.Run("unsorted blocks", func(b *testing.B) {
		priceBlocks := []PriceBlock{
			{Start: 60, Price: 30},
			{Start: 0, Price: 0},
			{Start: 85, Price: 0},
			{Start: 10, Price: 20},
		}

		b.ResetTimer()
		for i := 0; i < b.N; i++ {
			_ = svc.createPriceBlockLookup(priceBlocks)
		}
	})

	b.Run("minute-by-minute blocks", func(b *testing.B) {
		// Worst case: every minute is a new block
		priceBlocks := make([]PriceBlock, 1440)
		for i := 0; i < 1440; i++ {
			priceBlocks[i] = PriceBlock{
				Start: i,
				Price: i % 100,
			}
		}

		b.ResetTimer()
		for i := 0; i < b.N; i++ {
			_ = svc.createPriceBlockLookup(priceBlocks)
		}
	})
}

type mockPriceBlockGetter struct {
	blocks []PriceBlock
}

func (m *mockPriceBlockGetter) GetPriceBlocks() []PriceBlock {
	return m.blocks
}

func BenchmarkGetPrice(b *testing.B) {
	b.Run("lookup single price", func(b *testing.B) {
		// Setup - not timed
		getter := &mockPriceBlockGetter{
			blocks: []PriceBlock{
				{Start: 0, Price: 0},
				{Start: 10, Price: 20},
				{Start: 60, Price: 30},
				{Start: 85, Price: 0},
			},
		}

		service := New(getter)
		someTime := time.Now()

		b.ResetTimer() // We only care about the runtime performance of GetPrice

		// Benchmark just the lookup
		for i := 0; i < b.N; i++ {
			_ = service.GetPrice(someTime) // Or whatever your method is called
		}
	})

	b.Run("lookup various minutes", func(b *testing.B) {
		getter := &mockPriceBlockGetter{
			blocks: []PriceBlock{
				{Start: 0, Price: 0},
				{Start: 10, Price: 20},
				{Start: 60, Price: 30},
				{Start: 85, Price: 0},
			},
		}

		service := New(getter)

		// Test different times, it doesn't matter if now + duration causes day overlap in this benchmark
		times := []time.Time{
			time.Now(),
			time.Now().Add(time.Minute * 20),
			time.Now().Add(time.Hour),
			time.Now().Add(time.Hour * 2),
			time.Now().Add(time.Hour * 6),
		}

		b.ResetTimer()

		for i := 0; i < b.N; i++ {
			t := times[i%len(times)]
			_ = service.GetPrice(t)
		}
	})

	b.Run("lookup with many blocks", func(b *testing.B) {
		// Create many price blocks for worst-case
		blocks := make([]PriceBlock, 144)
		for i := 0; i < 144; i++ {
			blocks[i] = PriceBlock{
				Start: i * 10,
				Price: i * 5,
			}
		}

		getter := &mockPriceBlockGetter{blocks: blocks}
		service := New(getter)
		someTime := time.Now()

		b.ResetTimer()

		for i := 0; i < b.N; i++ {
			_ = service.GetPrice(someTime)
		}
	})
}

func BenchmarkGetPrice_TenMillion(b *testing.B) {
	// Setup
	getter := &mockPriceBlockGetter{
		blocks: []PriceBlock{
			{Start: 0, Price: 0},
			{Start: 10, Price: 20},
			{Start: 60, Price: 30},
			{Start: 85, Price: 0},
		},
	}

	service := New(getter)

	someTime := time.Now()
	b.Run("sequential 10M calls", func(b *testing.B) {
		b.ResetTimer()

		for i := 0; i < b.N; i++ {
			// Call GetPrice exactly 10 million times
			for j := 0; j < 10_000_000; j++ {
				_ = service.GetPrice(someTime)
			}
		}
	})

	times := []time.Time{
		time.Now(),
		time.Now().Add(time.Minute * 20),
		time.Now().Add(time.Hour),
		time.Now().Add(time.Hour * 2),
		time.Now().Add(time.Hour * 6),
	}
	b.Run("varying minutes 10M calls", func(b *testing.B) {
		b.ResetTimer()

		for i := 0; i < b.N; i++ {
			for j := 0; j < 10_000_000; j++ {
				t := times[j%len(times)]
				_ = service.GetPrice(t)
			}
		}
	})
}
