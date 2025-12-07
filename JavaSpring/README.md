# Notes

Rebuilt in Java Spring Boot, to allow for easier configuration and maintenance. The solution can quite easily be made 
into plain Java if necessary; but the assumption is that holidays - and fees - will be updated using a dynamic data 
source eventually.

The defaults used in the previous code and specification have been recreated in the DefaultConfig; in splitting up the 
hard-coded hourly fee specification, it was discovered that only the second half hour in the hours between 7 and 15 
are billed. Verification needed to enure this is intentional.

Holidays for 2025 have been added for verification.

The fee calculation did not, in fact, check whether all passes were within one hour of another (due to only 
initializing interval once). A naive implementation - which only checks passes during the same hour - was used instead.

Unit tests were added to verify the functionality of the program. It is not exhaustive; but provides a baseline with 
the given data. 