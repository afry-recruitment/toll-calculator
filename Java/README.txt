
Some explanations to the changes with some help from Claude Code.

Bugs fixed
- Fee between 09:00-09:29, 10:00-10:29 ... 14:00-14:29 was 0 SEK. It is now 8 SEK, as for the rest of 08:30-14:59.
- "Charged once an hour" did not work: the start of the one-hour period never moved forward, so after
  the first hour every passage was charged. Passages are now grouped into one-hour periods, each period
  starting at the first passage after the previous period, and the highest fee in each period is charged.
- Passages given in any order are now sorted before the fee is calculated.
- Holidays were only known for 2013. They are now calculated for any year (see Holidays below).
- Times were read in the timezone of the computer running the code. On a server set to UTC, a passage
  at 07:30 Swedish time was charged as 06:30. All times are now read in Swedish time (Europe/Stockholm),
  including summer time.

New files
- VehicleType.java: enum with all vehicle types (CAR, MOTORBIKE, TRACTOR, EMERGENCY, DIPLOMAT, FOREIGN,
  MILITARY). Exists so that vehicle types are checked by the compiler instead of compared as strings,
  where a typo like "car" instead of "Car" would silently give the wrong fee. It only says what a
  vehicle is, not whether it is toll-free, so the toll rules stay inside the calculator.
- FeeInterval.java: record with start time, end time and fee for one part of the day. Exists so the fee
  table is data instead of a long chain of if-statements, and can be changed without changing code.
- TollConfiguration.java: all settings in one place (charged vehicle types, timezone, fee intervals,
  maximum fee per day and whether July is free), with defaults() for the current rules. Exists so the
  rules can be changed or tested with other values without changing the calculator.
- DateHelper.java: decides if a date is toll-free (weekend, Swedish public holiday, day before a
  holiday, July). Exists to keep the holiday logic, including the Easter calculation, separate from
  the fee calculation, so each part is smaller and easier to read.
- TollCalculatorTest.java: unit tests for all requirements (see Unit tests below). Exists because the
  original code had no tests, which is why its bugs were not found.
- lib/junit-platform-console-standalone-6.1.3.jar: JUnit, needed to compile and run the tests. Included
  because the project has no build tool (Maven or Gradle), so the tests can run right after cloning.

Holidays (Sweden)
- Weekends are free.
- Fixed public holidays: Jan 1, Jan 6, May 1, Jun 6, Dec 25, Dec 26.
- Easter-based public holidays: Good Friday, Easter Monday, Ascension Day and Whit Sunday, calculated
  from Easter Sunday for each year.
- Why Easter Sunday is important: most holidays that move between years are a fixed number of days
  from Easter Sunday (Good Friday -2, Easter Monday +1, Ascension Day +39, Whit Sunday +49). Easter
  itself depends on the moon and falls between March 22 and April 25, so it must be calculated for
  each year. If Easter is wrong, five weekdays a year get the wrong fee: Maundy Thursday, Good Friday,
  Easter Monday, the Wednesday before Ascension Day and Ascension Day.
- Midsummer Day (Saturday between Jun 20-26) and All Saints' Day (Saturday between Oct 31-Nov 6).
- The day before a public holiday is also free, for example Maundy Thursday, Apr 30, Midsummer Eve,
  Dec 24 and Dec 31.
- July is free (general summer holiday). This can be turned off in the configuration.
- All holidays are calculated from the year of the passage, so nothing needs to be changed when a new
  year starts. There is no list of dates that has to be updated every year, and no risk that the
  calculator charges fees on holidays because someone forgot to update it.
- Alternatives would be to use a third-party library to handle the dynamic holidays (for example
  Jollyday), or to keep the holiday dates in the configuration and update them every year. Calculating
  them was chosen because it needs no extra dependency and no yearly maintenance.
- Sources:
  - Easter Sunday is calculated with the "Anonymous Gregorian algorithm", also known as the
    Meeus/Jones/Butcher algorithm. 
  - The Swedish public holidays are defined in Lag (1989:253) om allmanna helgdagar.
  - That the day before a public holiday and July are free comes from the Swedish congestion tax rules,
    Lag (2004:629) om trangselskatt, and matches the hardcoded 2013 dates in the original code.

API changes
- New method getTollFee(Vehicle vehicle, Instant... instants). Instant is used instead of Date.
- getTollFee(Vehicle vehicle, Date... dates) is kept for existing callers but marked @Deprecated.
  It converts the dates to Instants and gives the same result.
- The single-passage method getTollFee(Date date, Vehicle vehicle) is removed.
  Use getTollFee(vehicle, instant) instead.
- Vehicle.getType() returns the enum VehicleType instead of a String, so vehicle types cannot be misspelled.
- Which vehicle types are toll-free is decided inside the calculator, not by the vehicle.

Input validation
- A null vehicle, a null list of passages or a null passage throws IllegalArgumentException
  (for both the Instant and the Date method).
- Passages on different days throw IllegalArgumentException, since the fee is calculated per day.
- No passages gives 0 SEK.

Configuration
- New class TollConfiguration with:
  - vehicle types that are charged (default: CAR, all other types are toll-free)
  - timezone of the toll stations (default: Europe/Stockholm)
  - fee intervals with start time, end time and fee (default: the current fee table)
  - maximum fee per day (default: 60 SEK)
  - whether July is toll-free (default: yes)
- new TollCalculator() uses the default configuration.
- new TollCalculator(TollConfiguration configuration) uses the given configuration.
- Fee intervals are given with minute precision, start included and end excluded,
  for example 06:30-07:00 = 13 SEK. Times not covered by an interval are free.

Unit tests
- New test class TollCalculatorTest (JUnit 6), 205 tests, all passing.
- One group of tests for each requirement: fee by time of day, rush hour, maximum per day,
  once per hour, toll-free vehicles, weekends and holidays.
- Holidays are tested for 2026, 2027, 2028, 2030, 2032 and 2035, including the leap years 2028 and 2032.
- Also tested: Swedish summer and winter time, invalid input, no passages, the deprecated Date method,
  custom configuration, turning the July holiday on and off, and that the default configuration
  matches the requirements.

How to run the tests (from the Java folder)
  javac -d out -cp lib/junit-platform-console-standalone-6.1.3.jar *.java
  java -jar lib/junit-platform-console-standalone-6.1.3.jar execute --class-path out --select-class TollCalculatorTest

Assumptions
- Holidays follow Swedish rules, including the day before a holiday and July.
- A passage exactly 60 minutes after the start of a one-hour period starts a new period.
- The configuration is not validated yet (for example overlapping fee intervals or unknown vehicle types).
