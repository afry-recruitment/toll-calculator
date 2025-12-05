.PHONY: test build run-demo clean

# Binary name
BINARY_NAME=tollcalculator

# Default target
all: build

# Run all tests
test:
	@echo "Running tests..."
	go test -v ./...

# Build the binary
build:
	@echo "Building binary..."
	go build -o $(BINARY_NAME) .

# Run the demo application
run-demo: build
	@echo "Running demo..."
	./$(BINARY_NAME)

# Clean build artifacts
clean:
	@echo "Cleaning..."
	rm -f $(BINARY_NAME)
	rm -f coverage.out
