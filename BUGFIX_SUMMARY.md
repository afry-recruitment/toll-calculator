# Bug Fixes - Legacy Code

## Summary

Found bugs in the original code after adding tests to verify the criteria of the calculator


| Bug | Severity | Impact |
|-----|----------|--------|
| #1 - Time calc | Critical | Rolling windows didn't work |
| #2 - Window update | Critical | Multi-window scenarios didn't work |
| #3 - Time range | High | Wrong fees 9:00-14:29 (0 instead of 8 SEK) |
| #4 - Precedence | Medium | Overcharged 15:00-15:29 by 5 SEK |
| #5 - Accumulation | High | Wrong totals for same-window passages |

---

## Tests Added

**Bug Verification:**
- Time difference calculation (60+ minutes)
- Window boundary behavior
- Mid-day time range (8:30-14:59)
- Afternoon time range (15:00-16:59)
- Fee accumulation logic

**Core Functionality:**
- Empty/null inputs
- Toll-free vehicles, weekends, holidays
- Daily cap scenarios
- All time brackets
- Multiple passages and windows
- Unsorted input handling

---
