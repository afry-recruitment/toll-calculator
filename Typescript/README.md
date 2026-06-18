# Toll Calculator

## Installation
npm install

## Run tests
npm test

## Notes

I tried to keep the project small and functional but still forward-thinking.

**TOLL_INTERVALS** is hardcoded for now but in production I'd fetch it from a data source, that way the fees can be updated without a redeploy.

**Strict types** I split vehicles into `TollFreeVehicle` and `TollableVehicle` for readability and to make it harder to pass in wrong values at runtime.

**Single day assumption** I assume all passages belong to the same day, e.g. from a nightly batch job processing today's passages. The daily max of 60 SEK is applied per call.

**Sliding window** handles the "once per hour, highest fee wins" rule.

**Holidays** the hardcoded 2013 list looked like a bug so I replaced it with `date-holidays`. No need to reinvent the wheel.

**Timezone** In production I would use a set timezone for where the application is used.

## Bug fixes from original

- Price intervals had gaps, e.g. 09:15 returned 0 SEK instead of 8 SEK
- 60-minute window never reset, so only the first window worked correctly
- Hardcoded 2013 holidays and July
