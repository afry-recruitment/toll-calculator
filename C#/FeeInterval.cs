using System;

/// <summary>
/// A single window in the toll fee schedule and the fee charged for a pass
/// that falls inside it. <see cref="Start"/> and <see cref="End"/> are inclusive.
/// </summary>
public readonly record struct FeeInterval(TimeOnly Start, TimeOnly End, int Fee);
