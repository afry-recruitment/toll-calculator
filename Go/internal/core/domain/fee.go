package domain

import (
	"time"
)

type Fee struct {
	StartHour    int     `json:"start_hour"`
	StartMinutes int     `json:"start_minutes"`
	EndHour      int     `json:"end_hour"`
	EndMinutes   int     `json:"end_minutes"`
	Fee          float64 `json:"fee"`
}

func (f *Fee) Validate() bool {
	if f.Fee < MinFeeValue || f.Fee > MaxFeeValue {
		return false
	}

	if f.StartHour < 0 || f.StartHour > 23 {
		return false
	}

	if f.EndHour < 0 || f.EndHour > 23 {
		return false
	}

	if f.EndHour < f.StartHour {
		return false
	}

	if f.StartMinutes < 0 || f.StartMinutes > 59 {
		return false
	}

	if f.EndMinutes < 0 || f.EndMinutes > 59 {
		return false
	}

	return true
}

func (f Fee) Check(pass time.Time) bool {
	passHour, passMinutes := pass.Hour(), pass.Minute()

	if passHour < f.StartHour || passHour > f.EndHour {
		return false
	}

	if passHour == f.StartHour && passMinutes < f.StartMinutes {
		return false
	}

	if passHour == f.EndHour && passMinutes > f.EndMinutes {
		return false
	}

	return true
}
