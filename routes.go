package main

import (
	"log/slog"
	"net/http"

	"afry-toll-calculator/metrics"
	"github.com/prometheus/client_golang/prometheus/promhttp"

	"afry-toll-calculator/handlers"
	"afry-toll-calculator/services/fee"
)

func routes(feeService fee.Service) http.Handler {
	mux := http.NewServeMux()
	mux.HandleFunc("/fee", handlers.GetFeeHandler(feeService))
	mux.HandleFunc("/health", func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Content-Type", "application/json")
		w.WriteHeader(http.StatusOK)
		_, err := w.Write([]byte(`{"status":"ok"}`))
		if err != nil {
			slog.Error("failed to write health check response", "error", err)
		}
	})
	mux.HandleFunc("/health/ready", func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Content-Type", "application/json")
		w.WriteHeader(http.StatusOK)
		_, err := w.Write([]byte(`{"status":"ok"}`))
		if err != nil {
			slog.Error("failed to write readyness check response", "error", err)
		}
	})
	mux.HandleFunc("/health/live", func(w http.ResponseWriter, r *http.Request) {
		w.Header().Set("Content-Type", "application/json")
		w.WriteHeader(http.StatusOK)
		_, err := w.Write([]byte(`{"status":"alive"}`))
		if err != nil {
			slog.Error("failed to write liveness check response", "error", err)
		}
	})
	mux.Handle("/metrics", promhttp.Handler())

	return metrics.Middleware(mux)
}
