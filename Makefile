.PHONY: help
help:
	@echo "Available commands:"
	@echo "  make run           - Run locally"
	@echo "  make build         - Build binary"
	@echo "  make test          - Run tests"
	@echo "  make bench         - Run benchmarks"
	@echo "  make docker-build  - Build Docker image"
	@echo "  make docker-run    - Run container in docker"
	@echo "  make docker-up     - Start docker-compose"
	@echo "  make docker-down   - Stop docker-compose"
	@echo "  make hey-load-test - Run load test on localhost:3000 (requires hey)"
	@echo "  make load-test     - Run go load test simulator (courtesy of Claude Code)"
	@echo "  make golangci-lint - Run golangci-lint"
	@echo "  make vuln-check    - Run vulnerability check (requires govulncheck)"

.PHONY: run
run:
	go run . -mod=vendor

.PHONY: build
build:
	go build -mod=vendor -o bin/toll-calculator .

.PHONY: test
test:
	go test -mod=vendor -v -race -cover ./...

.PHONY: bench
bench:
	go test -mod=vendor -bench=. -benchmem -benchtime=3s ./...

.PHONY: mocks
mocks:
	mockery

.PHONY: docker-build
docker-build:
	docker build -t toll-calculator:latest .

.PHONY: docker-run
docker-run:
	docker run -p3000:3000 toll-calculator:latest

.PHONY: docker-up
docker-up:
	docker compose up -d

.PHONY: docker-down
docker-down:
	docker compose down -v

.PHONY: hey-load-test
hey-load-test:
	hey -n 500000 -c 500 -m POST \
	-H "Content-Type: application/json" \
	-d '{"vehicleType":"car","timestamps":["2025-12-05T06:30:00Z","2025-12-05T07:30:00Z","2025-12-05T08:30:00Z","2025-12-05T09:30:00Z"]}' \
	http://localhost:3000/fee

.PHONY: load-test
load-test:
	go run util/loadtest.go

.PHONY: golangci-lint
golangci-lint:
	golangci-lint run

vuln-check:
	govulncheck ./...
