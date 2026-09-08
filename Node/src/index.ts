import { passageTime } from "../test/helper/helper.js";
import { GetTollFeeForDates } from "./tollCalculator.js";
const multiplePassages = [
  passageTime({ time: "06:30", day: 8, month: 8, year: 2026 }),
  passageTime({ time: "07:00", day: 8, month: 8, year: 2026 }),
  passageTime({ time: "08:30", day: 8, month: 8, year: 2026 }),
  passageTime({ time: "15:30", day: 8, month: 8, year: 2026 }),
  passageTime({ time: "17:00", day: 1, month: 8, year: 2026 }),
  passageTime({ time: "18:01", day: 10, month: 8, year: 2026 }),
];
/* eslint-disable */
//@ts-ignore
const fee = GetTollFeeForDates({ vehicle: "Car", dates: multiplePassages });
