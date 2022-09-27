package fees

import "github.com/cmoscofian/toll-calculator/internal/core/domain"

type inMemory struct {
	fees []domain.Fee
}

func NewInMemory() *inMemory {
	return &inMemory{
		fees: []domain.Fee{
			{StartHour: 6, StartMinutes: 0, EndHour: 6, EndMinutes: 29, Fee: 8},
			{StartHour: 6, StartMinutes: 30, EndHour: 6, EndMinutes: 59, Fee: 13},
			{StartHour: 7, StartMinutes: 0, EndHour: 7, EndMinutes: 59, Fee: 18},
			{StartHour: 8, StartMinutes: 0, EndHour: 8, EndMinutes: 29, Fee: 13},
			{StartHour: 8, StartMinutes: 30, EndHour: 14, EndMinutes: 59, Fee: 8},
			{StartHour: 15, StartMinutes: 0, EndHour: 15, EndMinutes: 29, Fee: 13},
			{StartHour: 15, StartMinutes: 30, EndHour: 16, EndMinutes: 59, Fee: 18},
			{StartHour: 17, StartMinutes: 0, EndHour: 17, EndMinutes: 59, Fee: 13},
			{StartHour: 18, StartMinutes: 0, EndHour: 18, EndMinutes: 29, Fee: 8},
		},
	}
}

func (i inMemory) FetchFees() []domain.Fee {
	return i.fees
}
