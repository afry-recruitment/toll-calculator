# Toll fee calculator 1.2.1

### Installation

First, if you want to load holiday lists for any other regions, other than the ones provided at app/data/holidays_*. you need a google api key to get up-to-date holiday calendar and/or one matching a different region. Checkout https://developers.google.com/calendar/api/guides/auth for details on generating one.

Open file _settings/secrets.properties_.

Add property

> GOOGLE_CALENDAR_API_KEY=${your google api key}

Then at path _/java/_ run:

> gradlew app:buildExecutable

This will create a runnable fat jar with settings and data in _app/build/executable_.

Note: if you run from IDE or terminal/.class files make sure that you have _java/app/_ as working directory.

### Running

> java -jar [COMMAND] [ARGUMENTS...]

example:

> java -jar help

or:

> java -jar calculate -in data/toll-passes.csv -r sweden

### Upcoming:

- Generalised intervals (instead of just hour/biggest fee and day/fee roof)
- Fractional prices (currently only integers)
- Currency units
- Wider command support
- Persistent CLI application
- Better reports (which passage are you paying for, which are free)
- DB with ability to apply fee consistently between app runs.
- Better multi-threading
- More tests

## Toll fee calculator 1.2.1

### Fixes:

- Refactored TollFeeCalculator.getTollfee and added HourFee, DayFee instead of DayInterval and 
  HourInterval. These replacements will handle most their corresponding logic when calculating tollfee. 
  Also keeps and exposes more information for report making.
- Default and Unknown command types now give responses. Default - when no command is given. Unknown - when 
  no command can be interpreted.

## Toll fee calculator 1.2

### Fixes:

- Report system.
- Service interfaces for TollCalculator supporting services to: make consistent testing easier, allow for 
  easier updates in the future.
- Use apache commons csv parser.
- Multithreaded csv parser.
- Unique vehicles.
- Externalised vehicle types to csv
- Parse csv data files and make reports based on calculations.
- Command system (Not fully implemented).
- Updated tests

