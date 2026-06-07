Feature: GetTollFee


Scenario Outline: Gets default toll for dates and vehicle type
	Given the vehicle is <vehicleType>
	And the dates are <dates>
	When the toll is calculated
	Then the result should be <fee>

Examples:
	| vehicleType | fee | dates                                                                                                      |
	| 'MotorBike' |   8 | '2026-06-03, 6:02:13; 2026-06-03, 6:05:13; 2026-06-03, 6:09:13'                                            |
	| 'MotorBike' |  16 | '2026-06-03, 6:02:13; 2026-06-03, 6:05:13; 2026-06-04, 6:09:13'                                            |
	| 'MotorBike' |  13 | '2026-06-03, 6:02:13; 2026-06-03, 6:50:13'                                                                 |
	| 'Car'       |   0 | '2026-06-07, 2:52:13'                                                                                      |
	| 'Tractor'   |  13 | '2026-06-02, 8:01:00'                                                                                      |
	| 'Military'  |   0 | '2026-06-02, 8:01:00'                                                                                      |
	| 'Car'       |  65 | '2026-06-03, 6:02:13; 2026-06-03, 7:05:13; 2026-06-04, 8:09:13; 2026-06-04, 9:09:13; 2026-06-04, 10:09:13' |
	| 'Car'       |  60 | '2026-06-03, 6:02:13; 2026-06-03, 7:05:13; 2026-06-03, 8:09:13; 2026-06-03, 9:09:13; 2026-06-03, 10:09:13' |
