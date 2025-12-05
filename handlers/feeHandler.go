package handlers

import (
	"encoding/json"
	"errors"
	"log/slog"
	"net/http"
	"time"

	"afry-toll-calculator/metrics"
	"afry-toll-calculator/models"
	"afry-toll-calculator/services/fee"
)

type FeeRequest struct {
	VehicleType string      `json:"vehicleType"`
	Timestamps  []time.Time `json:"timestamps"`
}

func GetFeeHandler(feeService fee.Service) http.HandlerFunc {
	return func(w http.ResponseWriter, r *http.Request) {
		var (
			err        error
			feeRequest FeeRequest
			fee        = 0
		)
		defer func() {
			metrics.RecordFeeCalculation(feeRequest.VehicleType, fee, err)
		}()

		if r.Method != http.MethodPost {
			http.Error(w, "invalid method", http.StatusMethodNotAllowed)
			return
		}

		err = json.NewDecoder(r.Body).Decode(&feeRequest)
		if err != nil {
			http.Error(w, "Invalid request body", http.StatusBadRequest)
			return
		}
		defer func() {
			erri := r.Body.Close()
			if erri != nil {
				slog.ErrorContext(r.Context(), "failed to close request body", "error", erri)
			}
		}()

		// Validate
		if feeRequest.VehicleType == "" {
			err = errors.New("missing vehicle type")
			http.Error(w, "missing vehicle type", http.StatusBadRequest)
			return
		}
		if len(feeRequest.Timestamps) == 0 {
			err = errors.New("missing timestamps array")
			http.Error(w, "missing timestamps array", http.StatusBadRequest)
			return
		}

		fee, err = feeService.GetFee(models.VehicleType(feeRequest.VehicleType), feeRequest.Timestamps)
		if err != nil {
			http.Error(w, "fee calculation failed", http.StatusInternalServerError)
			return
		}

		// Send response
		w.Header().Set("Content-Type", "application/json")
		err = json.NewEncoder(w).Encode(map[string]interface{}{
			"fee": fee,
		})
		if err != nil {
			slog.ErrorContext(r.Context(), "failed to encode response", "error", err)
		}
	}
}
