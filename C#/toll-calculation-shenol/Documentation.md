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
	6. Test the project with test driven development (TDD) approach
	
- 3. Extended Solution 
	1. Make the code more maintainable and extensible
	2. Using .NET for API endpoints and swagger UI documentation (Give support HTTPS API endpoints)
	3. Add Docker support for the project
	4. Add Kubernetes support for the project
	5. Add CI/CD pipeline for the project
	6. Add end-to-end/integration, etc. tests for the project
	7. Add more logging to the project
	8. Add more error handling to the project
	9. Add more validation to the project
	10. Add possible admin page for the project
	11. Could use Entity Framework Core for the project for database operations
	12. In any other case could use Repository pattern for the project to make it more maintainable and extensible


### Solution

- Created a new project with .sln file
- Created TEST cases to adopt TDD approach for the project 
- Deleted unnecessary files and folders from the project


I used tests to check my result are correct or not. I used TDD approach to develop the project. Thats why there is no API or Console application for the project. I just used tests to check my results. For proper solution should contains extended solution that I mentioned above but this project is only for test purposes. If I should develop for production, it could consume more time and effort and that's why I just used tests to check my results. From beginning I thought I could use API endpoints and swagger UI documentation but I decided to use tests to check my result because of the time constraints. There are totally 27 test cases that checks many pieces.