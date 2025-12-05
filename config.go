package main

type config struct {
	Host     string `envconfig:"HOST" default:"0.0.0.0"`
	Port     int    `envconfig:"PORT" default:"3000"`
	LogLevel string `envconfig:"LOG_LEVEL" default:"INFO"`
}
