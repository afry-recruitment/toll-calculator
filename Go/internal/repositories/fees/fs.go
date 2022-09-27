package fees

import (
	"encoding/json"
	"log"
	"os"

	"github.com/cmoscofian/toll-calculator/internal/core/domain"
)

type fs struct {
	path string
}

func NewFS(path string) *fs {
	return &fs{
		path: path,
	}
}

func (f fs) FetchFees() []domain.Fee {
	bs, err := os.ReadFile(f.path)
	if err != nil {
		log.Panicf("unable to load fees file from path %s due to %s", f.path, err.Error())
	}

	parsed := make([]domain.Fee, 0)
	if err := json.Unmarshal(bs, &parsed); err != nil {
		log.Panicf("unable to parse fees data due to %s", err.Error())
	}

	fees := make([]domain.Fee, 0, len(parsed))
	for _, fee := range parsed {
		if fee.Validate() {
			fees = append(fees, fee)
		}
	}

	return fees
}
