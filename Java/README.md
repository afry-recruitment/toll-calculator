# Toll fee calculator 1.1

### Installation

First you need a google api key to get up-to-date holiday calendar and/or one matching a different region.
Checkout https://developers.google.com/calendar/api/guides/auth for details on generating one.

Open file _settings/secrets.properties_.

Add property

> GOOGLE_CALENDAR_API_KEY=${your google api key}

Then at path _/java/_ run:

> gradlew app:buildExecutable

This will create a runnable fat jar with settings and data in _app/build/executable_.

Note: if you run from IDE or terminal/.class files make sure that you have _java/app/_ as working directory.

### Fixes:

- The hourly pass rate is now 8 between 8:30 - 15:00. Earlier every first half hour of each hour between 8 -
  15 was free.
- Holiday date where only checked for the year 2013 and did not even seem to be correct for that year. Check
  removed for now but the holiday data will not be correct.