/**
 * A fee band: every wall-clock minute in the inclusive range `[from, to]`
 * is charged `fee`. Bands in a {@link FeeSchedule} must not overlap; any
 * minute not covered by a band is implicitly fee-free.
 */
export interface FeeBand {
  /** Inclusive lower bound as "HH:MM". */
  readonly from: string;
  /** Inclusive upper bound as "HH:MM". */
  readonly to: string;
  /** Fee charged for any pass within the band, in the schedule's currency. */
  readonly fee: number;
}

/**
 * Ordered, non-overlapping list of fee bands. Wall-clock semantics are
 * defined by the time zone the calculator is configured with — the band
 * times are interpreted in that zone.
 */
export type FeeSchedule = readonly FeeBand[];

/**
 * Returns the fee for the given wall-clock hour/minute according to the
 * supplied schedule. Returns `0` for any time not covered by a band.
 *
 * Killing bug `[D§4.3]`: the schedule is plain data, so every minute is
 * either inside a band (with the band's fee) or outside all of them
 * (with `0`) — there is no `if`/`else` chain to forget a case.
 */
export const feeAt = (
  schedule: FeeSchedule,
  hour: number,
  minute: number,
): number => {
  const target = hour * 60 + minute;
  for (const band of schedule) {
    if (target >= minutesOfDay(band.from) && target <= minutesOfDay(band.to)) {
      return band.fee;
    }
  }
  return 0;
};

const minutesOfDay = (hhmm: string): number => {
  const [hh, mm] = hhmm.split(":");
  if (hh === undefined || mm === undefined) {
    throw new Error(`Invalid time format (expected "HH:MM"): ${hhmm}`);
  }
  return Number(hh) * 60 + Number(mm);
};
