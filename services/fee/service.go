package fee

import (
	"errors"
	"sort"
	"time"

	"afry-toll-calculator/integrations/dagsmart"
	"afry-toll-calculator/models"
	"afry-toll-calculator/services/pricelist"
	"afry-toll-calculator/services/vehiclelist"
)

type Service interface {
	GetFee(vehicleType models.VehicleType, entryDates []time.Time) (int, error)
}

func New(
	vehiclesGetter vehiclelist.Getter,
	dagsmartService dagsmart.Service,
	priceListService pricelist.Service,
) Service {
	vl := map[models.VehicleType]bool{}
	for _, v := range vehiclesGetter.GetVehicleList() {
		vl[v.GetType()] = v.IsTollFree()
	}

	svc := feeService{
		vehiclesGetter:   vehiclesGetter,
		holidaysGetter:   dagsmartService,
		priceListService: priceListService,
		vehicleLookup:    vl,
		publicHolidays:   map[int]map[string]struct{}{},
	}

	return &svc
}

type feeService struct {
	publicHolidays   map[int]map[string]struct{}
	vehiclesGetter   vehiclelist.Getter
	holidaysGetter   dagsmart.Service
	priceListService pricelist.Service
	vehicleLookup    map[models.VehicleType]bool
}

type billableBlock struct {
	start time.Time
	end   time.Time
	price int
}

// getHolidays retrieves and caches the list of public holidays for the specified year. Returns an error if retrieval fails.
// The purpose of this indirection is to ensure that when the year changes, service will self-manage retrieval and caching of
// holidays for the new year, otherwise stale data would be cached until service restart.
func (s *feeService) getHolidays(year int) (map[string]struct{}, error) {
	if _, ok := s.publicHolidays[year]; !ok {
		s.publicHolidays[year] = map[string]struct{}{}
		h, err := s.holidaysGetter.Get(year)
		if err != nil {
			return nil, err
		}

		for _, date := range h {
			s.publicHolidays[year][date] = struct{}{}
		}
	}

	return s.publicHolidays[year], nil
}

func (s *feeService) filterBillableDates(dates []time.Time) ([]time.Time, error) {
	out := dates[:0]
	for _, v := range dates {
		switch v.Weekday() {
		case time.Saturday, time.Sunday:
			continue
		default:
			date := v.Format(models.PUBLIC_HOLIDAY_DATE_FORMAT)
			h, err := s.getHolidays(v.Year())
			if err != nil {
				return nil, err
			}

			if _, ok := h[date]; ok {
				continue
			}

			out = append(out, v)
		}
	}

	return out, nil
}

func (s *feeService) validateSingleDay(entryDates []time.Time) bool {
	dates := map[string]struct{}{}
	for _, v := range entryDates {
		s := v.Format(models.PUBLIC_HOLIDAY_DATE_FORMAT)
		dates[s] = struct{}{}
	}

	return len(dates) == 1
}

// GetFee returns the total sum of fees for a given array of entry times. Function will return an
// error if entry times for more than one day are included.
func (s *feeService) GetFee(vehicleType models.VehicleType, entryDates []time.Time) (int, error) {
	if !s.validateSingleDay(entryDates) {
		return 0, errors.New("GetFee call contains more than one day of entry times")
	}

	tollFree, vehicleFound := s.vehicleLookup[vehicleType]
	if !vehicleFound {
		return 0, errors.New("unknown vehicle type")
	}

	if tollFree {
		return 0, nil
	}

	billableDates, err := s.filterBillableDates(entryDates)
	if err != nil {
		return 0, err
	}

	if len(billableDates) == 0 {
		return 0, nil
	}

	sort.Slice(billableDates, func(i, j int) bool {
		return billableDates[i].Before(billableDates[j])
	})

	var currentBlock *billableBlock
	billableBlocks := []*billableBlock{}
	for _, date := range billableDates {
		if currentBlock == nil || currentBlock.end.Before(date.Add(time.Minute)) {
			currentBlock = &billableBlock{date, date.Add(time.Hour), 0}
			billableBlocks = append(billableBlocks, currentBlock)
		}

		price := s.priceListService.GetPrice(date)
		if currentBlock.price < price {
			currentBlock.price = price
		}
	}

	sum := 0
	for _, block := range billableBlocks {
		sum += block.price
	}

	if sum > 60 {
		return 60, nil
	} else {
		return sum, nil
	}
}
