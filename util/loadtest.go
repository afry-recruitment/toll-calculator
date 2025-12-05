package main

import (
	"bytes"
	"encoding/json"
	"fmt"
	"io"
	"log/slog"
	"math/rand"
	"net/http"
	"sync"
	"time"
)

type FeeRequest struct {
	VehicleType string   `json:"vehicleType"`
	Timestamps  []string `json:"timestamps"`
}

var vehicleTypes = []struct {
	name   string
	weight int
}{
	{"car", 80},
	{"motorbike", 10},
	{"emergency", 5},
	{"military", 5},
}

func pickVehicleType() string {
	r := rand.Intn(100)
	cumulative := 0
	for _, v := range vehicleTypes {
		cumulative += v.weight
		if r < cumulative {
			return v.name
		}
	}
	return "car"
}

func generateTimestamps() []string {
	count := rand.Intn(15) + 1                      // 1-15 timestamps
	day := time.Now().AddDate(0, 0, -rand.Intn(30)) // Random day in last 30 days
	year, month, dayOfMonth := day.Date()

	timestamps := make([]string, count)
	for i := 0; i < count; i++ {
		hour := rand.Intn(24)
		minute := rand.Intn(60)
		t := time.Date(year, month, dayOfMonth, hour, minute, 0, 0, time.UTC)
		timestamps[i] = t.Format(time.RFC3339)
	}
	return timestamps
}

func generatePayload() ([]byte, bool) {
	// 5% chance of bad payload
	if rand.Intn(100) < 5 {
		badPayloads := []string{
			`{"vehicleType":"","timestamps":["2025-12-05T06:30:00Z"]}`,
			`{"vehicleType":"car","timestamps":[]}`,
			`{"vehicleType":"car"}`,
			`{"timestamps":["2025-12-05T06:30:00Z"]}`,
			`{invalid json`,
			`{"vehicleType":"unknown","timestamps":["2025-12-05T06:30:00Z"]}`,
		}
		return []byte(badPayloads[rand.Intn(len(badPayloads))]), true
	}

	req := FeeRequest{
		VehicleType: pickVehicleType(),
		Timestamps:  generateTimestamps(),
	}
	data, _ := json.Marshal(req)
	return data, false
}

func worker(id int, wg *sync.WaitGroup, requests <-chan struct{}, client *http.Client, url string, stats *Stats) {
	defer wg.Done()

	for range requests {
		payload, isBad := generatePayload()

		resp, err := client.Post(url, "application/json", bytes.NewReader(payload))
		if err != nil {
			stats.RecordError()
			continue
		}
		_, erri := io.Copy(io.Discard, resp.Body)
		if erri != nil {
			slog.Warn("failed to drain response body", "error", erri)
		}
		err = resp.Body.Close()
		if err != nil {
			slog.Warn("failed to close response body", "error", err)
		}

		stats.RecordRequest(resp.StatusCode, isBad)
	}
}

type Stats struct {
	mu         sync.Mutex
	total      int
	success    int
	badSent    int
	statusCode map[int]int
}

func (s *Stats) RecordRequest(status int, isBad bool) {
	s.mu.Lock()
	defer s.mu.Unlock()
	s.total++
	if status == 200 {
		s.success++
	}
	if isBad {
		s.badSent++
	}
	s.statusCode[status]++
}

func (s *Stats) RecordError() {
	s.mu.Lock()
	defer s.mu.Unlock()
	s.total++
	s.statusCode[-1]++
}

func (s *Stats) Print() {
	s.mu.Lock()
	defer s.mu.Unlock()
	fmt.Printf("\n--- Results ---\n")
	fmt.Printf("Total requests: %d\n", s.total)
	fmt.Printf("Successful (200): %d\n", s.success)
	fmt.Printf("Bad payloads sent: %d\n", s.badSent)
	fmt.Printf("\nStatus codes:\n")
	for code, count := range s.statusCode {
		if code == -1 {
			fmt.Printf("  connection error: %d\n", count)
		} else {
			fmt.Printf("  %d: %d\n", code, count)
		}
	}
}

func main() {
	url := "http://localhost:3000/fee"
	totalRequests := 500000
	concurrency := 200

	client := &http.Client{
		Timeout: 10 * time.Second,
		Transport: &http.Transport{
			MaxIdleConns:        concurrency,
			MaxIdleConnsPerHost: concurrency,
			MaxConnsPerHost:     concurrency,
			IdleConnTimeout:     90 * time.Second,
			DisableKeepAlives:   false,
			ForceAttemptHTTP2:   false,
		},
	}

	stats := &Stats{statusCode: make(map[int]int)}
	requests := make(chan struct{}, totalRequests)

	var wg sync.WaitGroup
	for i := 0; i < concurrency; i++ {
		wg.Add(1)
		go worker(i, &wg, requests, client, url, stats)
	}

	start := time.Now()
	fmt.Printf("Starting load test: %d requests, %d concurrency\n", totalRequests, concurrency)

	for i := 0; i < totalRequests; i++ {
		requests <- struct{}{}
	}
	close(requests)

	wg.Wait()
	elapsed := time.Since(start)

	stats.Print()
	fmt.Printf("\nTime: %s\n", elapsed)
	fmt.Printf("RPS: %.2f\n", float64(totalRequests)/elapsed.Seconds())
}
