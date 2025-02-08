# Toll Fee Calculator

## Overview
This project calculates toll fees for vehicles based on the time of day, vehicle type and other criteria.

## Implementation Details

Structure of the project:

```bash
TollFeeCalculator/
├── TollFeeCalculator.Application/
├── TollFeeCalculator.Domain/
├── TollFeeCalculator.Infrastructure/
├── TollFeeCalculator.API/
└── TollFeeCalculator.Tests/
```

- 1. Problem Analysis
Current Issues:

- No proper separation of concerns
- Hard-coded business rules
- Limited extensibility
- No proper dependency injection
- No unit tests
- No validation
- Primitive datetime calculations that could lead to bugs
- Hard-coded toll-free dates for 2013 only
- No proper error handling
- No logging
- No configuration management

- 2. Proper Solution 
	1. Create plan to make the code more maintainable and extensible
	2. Create a new empty project with .sln file
	3. Follow the SOLID principles
	4. Follow the Clean Code principles
	5. Using .NET for API endpoints and swagger UI documentation
	
- 3. Extended Solution 
	1. Make the code more maintainable and extensible
	1. Give support HTTPS API endpoints
	1. Add Docker support for the project
	1. Add Kubernetes support for the project
	1. Add CI/CD pipeline for the project
	1. Add end-to-end/integration, etc. tests for the project
	1. Add more logging to the project
	1. Add more error handling to the project
	1. Add more validation to the project
	1. Add possible admin page for the project