import type { Vehicle } from "./vehicle.js";

/**
 * Boundary validators for the public calculator entry point.
 *
 * Why these exist even though the TypeScript signatures already forbid
 * `null` / `undefined` / invalid `Date`:
 *
 * TypeScript's guarantees vanish at runtime. The realistic origins of
 * malformed input to a library like this are:
 *   - `JSON.parse(apiResponse)` — a remote contract change ships strings
 *     where we expected `Date`s.
 *   - A database row mapped by an under-typed ORM.
 *   - A plain-JS caller, or any code that erased types via `as any`.
 *
 * In all three cases the type system already let the input through. The
 * job of these `asserts` helpers is to turn the failure into a clear
 * error at the boundary — *before* the value reaches the windowing
 * algorithm, where the eventual `Cannot read properties of null` is
 * useless to whoever is debugging the caller.
 *
 * Implementation note: each helper takes `unknown`, so the checks have
 * genuine type overlap and ESLint's `no-unnecessary-condition` rule is
 * satisfied without disabling it. From the caller's perspective the
 * narrowing is a no-op (we pass a value that is already typed `Vehicle`
 * or `readonly Date[]`), but the runtime check still runs.
 */

export function assertVehicle(value: unknown): asserts value is Vehicle {
  if (value === null || value === undefined) {
    throw new TypeError(
      "feeFor: `vehicle` is required (received null/undefined)",
    );
  }
  if (typeof value !== "object" || !("type" in value)) {
    throw new TypeError(
      "feeFor: `vehicle` must be an object with a `type` property",
    );
  }
}

export function assertPasses(value: unknown): asserts value is readonly Date[] {
  if (!Array.isArray(value)) {
    throw new TypeError("feeFor: `passes` must be an array");
  }
  for (let i = 0; i < value.length; i++) {
    const pass: unknown = value[i];
    if (!(pass instanceof Date) || Number.isNaN(pass.getTime())) {
      throw new RangeError(`feeFor: passes[${String(i)}] is not a valid Date`);
    }
  }
}
