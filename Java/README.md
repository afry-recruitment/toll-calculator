# Toll fee calculator 1.1

### Installation

Make file:

app/src/main/resources/secrets.properties

Add property GOOGLE_CALENDAR_API_KEY=${your google api key with access to google calendar here}

### Fixes:
- The hourly pass rate is now 8 between 8:30 - 15:00. Earlier every first half hour of each hour between 8 -
15 was free.
- Holiday date where only checked for the year 2013 and did not even seem to be correct for that year. 
  Check removed for now but the holiday data will not be correct.