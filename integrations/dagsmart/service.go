// Package dagsmart provides an integration with dagsmart.se API to fetch public holidays
// for a given year.
package dagsmart

import (
	"encoding/json"
	"errors"
	"fmt"
	"io"
	"log/slog"
	"net/http"
	"time"

	"afry-toll-calculator/models"
)

type HttpGetter interface {
	Get(url string) (*http.Response, error)
}

type Service interface {
	Get(year int) ([]string, error)
}

func New(httpService HttpGetter) Service {
	return &svc{
		httpService: httpService,
	}
}

type svc struct {
	httpService HttpGetter
}

type dagsmartItem struct {
	Date string `json:"date"`
	Code string `json:"code"`
	Name struct {
		En string `json:"en"`
		Sv string `json:"sv"`
	} `json:"name"`
}

// Get fetches and returns a list of public holiday dates as strings. Returns an error if fetching fails.
func (s *svc) Get(year int) ([]string, error) {
	items, err := s.getItems(year)
	if err != nil {
		return nil, err
	}

	var dates = make([]string, len(items))
	for i, item := range items {
		dates[i] = item.Date
	}

	if !s.validateDates(dates) {
		return nil, errors.New("failed to validate item date format")
	}

	return dates, nil
}

func (s *svc) getItems(year int) ([]dagsmartItem, error) {
	slog.Info("fetching holidays", slog.Int("year", year))

	res, err := s.httpService.Get(fmt.Sprintf("https://api.dagsmart.se/holidays?weekends=false&year=%d", year))
	if err != nil {
		return nil, err
	}
	defer func() {
		erri := res.Body.Close()
		if erri != nil {
			slog.Error("failed to close response body", "error", erri)
		}
	}()

	b, err := io.ReadAll(res.Body)
	if err != nil {
		return nil, err
	}

	var items []dagsmartItem
	err = json.Unmarshal(b, &items)
	if err != nil {
		return nil, errors.New("failed to unmarshal JSON response")
	}

	return items, nil
}

func (s *svc) validateDates(dates []string) bool {
	for _, v := range dates {
		if _, err := time.Parse(models.PUBLIC_HOLIDAY_DATE_FORMAT, v); err != nil {
			return false
		}
	}

	return true
}
