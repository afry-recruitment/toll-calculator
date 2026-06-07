Feature: Builder


Scenario Outline: Using the builder
	Given A builder is created
	And <holiday> dates are added
	And <freeVehicles> added
	And daily fee <limit> set
	And <fees> for times are added
	When the <vehicleType> is set
	And the <dates> are set
	And a calculation is performed
	Then the config is used and the result is <result>

Examples:
	| holiday | freeVehicles | limit | fees             | vehicleType | dates                 | result |
	| ''      | ''           |     2 | '8-10:5;11-12:6' | 'Car'       | '2026-06-02, 8:04:05' |      2 |
	| ''      | 'Car'        |     2 | '8-10:5;11-12:6' | 'Car'       | '2026-06-02, 8:04:05' |      0 |
	| ''      | 'Car'        |    10 | '8-10:5;11-12:6' | 'MotorBike' | '2026-06-02, 8:04:05' |      5 |
	| '06-02' | ''           |    10 | '8-10:5;11-12:6' | 'MotorBike' | '2026-06-02, 8:04:05' |      0 |
