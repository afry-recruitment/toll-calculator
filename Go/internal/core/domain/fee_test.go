package domain

import (
	"testing"
	"time"
)

func TestValidateFee(t *testing.T) {
	t.Parallel()
	t.Run("should invalidate fee with negative hour", func(t *testing.T) {
		t.Parallel()
		fee := Fee{StartHour: -100, Fee: MinFeeValue}
		if fee.Validate() {
			t.FailNow()
		}
	})

	t.Run("should invalidate fee set with too big hour value", func(t *testing.T) {
		t.Parallel()
		fee := Fee{EndHour: 100, Fee: MaxFeeValue}
		if fee.Validate() {
			t.FailNow()
		}
	})

	t.Run("should invalidate fee with negative minutes", func(t *testing.T) {
		t.Parallel()
		fee := Fee{StartMinutes: -1000, Fee: MinFeeValue}
		if fee.Validate() {
			t.FailNow()
		}
	})

	t.Run("should invalidate fee set with too large minutes value", func(t *testing.T) {
		t.Parallel()
		fee := Fee{EndMinutes: 68, Fee: MaxFeeValue}
		if fee.Validate() {
			t.FailNow()
		}
	})

	t.Run("should invalidate fee when start hour is bigger than end hour", func(t *testing.T) {
		t.Parallel()
		fee := Fee{StartHour: 10, EndHour: 5, Fee: MaxFeeValue}
		if fee.Validate() {
			t.FailNow()
		}
	})

	t.Run("should invalidate fee when value is under fee min limit", func(t *testing.T) {
		t.Parallel()
		fee := Fee{StartHour: 10, EndHour: 5, Fee: MinFeeValue - 1}
		if fee.Validate() {
			t.FailNow()
		}
	})

	t.Run("should invalidate fee when value is above fee max limit", func(t *testing.T) {
		t.Parallel()
		fee := Fee{StartHour: 10, EndHour: 5, Fee: MaxFeeValue + 1}
		if fee.Validate() {
			t.FailNow()
		}
	})

	t.Run("should validate fee when all validations pass", func(t *testing.T) {
		t.Parallel()
		fee := Fee{StartHour: 5, EndHour: 5, StartMinutes: 0, EndMinutes: 0, Fee: MinFeeValue}
		if !fee.Validate() {
			t.FailNow()
		}
	})
}

func TestCheckFee(t *testing.T) {
	t.Parallel()
	fee := Fee{StartHour: 12, EndHour: 12, StartMinutes: 10, EndMinutes: 50, Fee: MinFeeValue}

	t.Run("should not match a pass with previous hours", func(t *testing.T) {
		t.Parallel()
		pass := time.Date(2022, time.January, 1, 11, 0, 0, 0, time.UTC)
		if fee.Check(pass) {
			t.FailNow()
		}
	})

	t.Run("should not match a pass with after hours", func(t *testing.T) {
		t.Parallel()
		pass := time.Date(2022, time.January, 1, 13, 0, 0, 0, time.UTC)
		if fee.Check(pass) {
			t.FailNow()
		}
	})

	t.Run("should not match a pass with previous minutes", func(t *testing.T) {
		t.Parallel()
		pass := time.Date(2022, time.January, 1, 12, 5, 0, 0, time.UTC)
		if fee.Check(pass) {
			t.FailNow()
		}
	})

	t.Run("should not match a pass with after minutes", func(t *testing.T) {
		t.Parallel()
		pass := time.Date(2022, time.January, 1, 12, 55, 0, 0, time.UTC)
		if fee.Check(pass) {
			t.FailNow()
		}
	})
}
