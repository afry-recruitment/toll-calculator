package metrics

import (
	"net/http"
	"strconv"
	"time"

	"github.com/prometheus/client_golang/prometheus"
	"github.com/prometheus/client_golang/prometheus/promauto"
)

var (
	// HTTP metrics
	httpRequestsTotal = promauto.NewCounterVec(
		prometheus.CounterOpts{
			Name: "http_requests_total",
			Help: "Total number of HTTP requests",
		},
		[]string{"method", "path", "status"},
	)

	httpRequestDuration = promauto.NewHistogramVec(
		prometheus.HistogramOpts{
			Name:    "http_request_duration_seconds",
			Help:    "HTTP request duration in seconds",
			Buckets: []float64{.001, .005, .01, .025, .05, .1, .25, .5, 1, 2.5, 5},
		},
		[]string{"method", "path", "status"},
	)

	// Business metrics
	feeCalculationsTotal = promauto.NewCounterVec(
		prometheus.CounterOpts{
			Name: "fee_calculations_total",
			Help: "Total number of fee calculations",
		},
		[]string{"vehicle_type", "status"},
	)

	feeAmount = promauto.NewHistogramVec(
		prometheus.HistogramOpts{
			Name:    "fee_amount",
			Help:    "Distribution of calculated fees",
			Buckets: []float64{0, 10, 25, 50, 100, 200, 500, 1000},
		},
		[]string{"vehicle_type"},
	)
)

// Middleware wraps an http.Handler and records metrics
func Middleware(next http.Handler) http.Handler {
	return http.HandlerFunc(func(w http.ResponseWriter, r *http.Request) {
		start := time.Now()

		// Wrap response writer to capture status code
		wrapped := &responseWriter{ResponseWriter: w, statusCode: http.StatusOK}

		next.ServeHTTP(wrapped, r)

		duration := time.Since(start).Seconds()
		status := strconv.Itoa(wrapped.statusCode)

		httpRequestsTotal.WithLabelValues(r.Method, r.URL.Path, status).Inc()
		httpRequestDuration.WithLabelValues(r.Method, r.URL.Path, status).Observe(duration)
	})
}

// responseWriter wraps http.ResponseWriter to capture status code
type responseWriter struct {
	http.ResponseWriter
	statusCode int
}

func (rw *responseWriter) WriteHeader(code int) {
	rw.statusCode = code
	rw.ResponseWriter.WriteHeader(code)
}

// RecordFeeCalculation records a fee calculation metric
func RecordFeeCalculation(vehicleType string, fee int, err error) {
	status := "success"
	if err != nil {
		status = "error"
	}

	feeCalculationsTotal.WithLabelValues(vehicleType, status).Inc()

	if err == nil {
		feeAmount.WithLabelValues(vehicleType).Observe(float64(fee))
	}
}
