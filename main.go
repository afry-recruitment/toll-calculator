package main

import (
	"context"
	"errors"
	"fmt"
	"log/slog"
	"net/http"
	"os"
	"os/signal"
	"syscall"
	"time"

	"github.com/kelseyhightower/envconfig"

	"afry-toll-calculator/integrations/dagsmart"
	"afry-toll-calculator/services/fee"
	"afry-toll-calculator/services/pricelist"
	"afry-toll-calculator/services/vehiclelist"
)

func main() {
	ctx, cancel := context.WithCancel(context.Background())
	defer cancel()

	var cfg config
	err := envconfig.Process("TOLL_CALCULATOR", &cfg)
	if err != nil {
		slog.ErrorContext(ctx, "failed to process env config", "error", err)
		panic(err)
	}

	logger := slog.New(slog.NewJSONHandler(os.Stdout, &slog.HandlerOptions{
		Level: getLogLevel(cfg.LogLevel),
	}))
	slog.SetDefault(logger.With(slog.String("service", "toll-calculator")))

	feeService := fee.New(
		vehiclelist.NewHardcodedGetter(),
		dagsmart.New(http.DefaultClient),
		pricelist.New(&pricelist.HardcodedPriceBlocksGetter{}),
	)

	s := &http.Server{
		Addr:              fmt.Sprintf("%s:%d", cfg.Host, cfg.Port),
		ReadTimeout:       15 * time.Second,
		ReadHeaderTimeout: 5 * time.Second,
		WriteTimeout:      15 * time.Second,
		IdleTimeout:       120 * time.Second,
		MaxHeaderBytes:    50 << 10, // 50KB
		Handler:           routes(feeService),
	}

	serverErrors := make(chan error)
	go func() {
		slog.Info("starting http server",
			"addr", s.Addr,
		)
		serverErrors <- s.ListenAndServe()
	}()

	shutdown := make(chan os.Signal, 1)
	signal.Notify(shutdown, os.Interrupt, syscall.SIGTERM)

	select {
	case err := <-serverErrors:
		if err != nil && !errors.Is(err, http.ErrServerClosed) {
			slog.ErrorContext(ctx, "http server failed", "error", err)
			os.Exit(1)
		}

	case sig := <-shutdown:
		slog.Info("shutdown signal received", "signal", sig)

		ctx, cancel := context.WithTimeout(ctx, time.Second*15)
		defer cancel()

		if err := s.Shutdown(ctx); err != nil {
			slog.ErrorContext(ctx, "graceful shutdown failed", "error", err)

			if err := s.Close(); err != nil {
				slog.ErrorContext(ctx, "failed to shut down server forcefully", "error", err)
				os.Exit(1)
			}
		}

		slog.Info("server stopped gracefully")
	}
}

func getLogLevel(level string) slog.Level {
	switch level {
	case "DEBUG":
		return slog.LevelDebug
	case "INFO":
		return slog.LevelInfo
	case "WARN":
		return slog.LevelWarn
	case "ERROR":
		return slog.LevelError
	default:
		return slog.LevelInfo
	}
}
