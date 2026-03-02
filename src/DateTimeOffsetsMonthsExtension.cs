using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Soenneker.Extensions.DateTimeOffsets.Months;

/// <summary>
/// Provides extension methods for <see cref="DateTimeOffset"/> that operate on month boundaries,
/// including helpers that compute month starts/ends in a specified time zone while returning UTC instants.
/// </summary>
public static class DateTimeOffsetsMonthsExtension
{
    private const long _oneTick = 1;

    /// <summary>
    /// Returns the start of the month containing <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <remarks>No time zone conversion is performed and the offset is preserved.</remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToStartOfMonth(this DateTimeOffset dateTimeOffset)
        => new(dateTimeOffset.Year, dateTimeOffset.Month, 1, 0, 0, 0, dateTimeOffset.Offset);

    /// <summary>
    /// Returns the end of the month containing <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <remarks>
    /// Computed as one tick before the start of the next month. No time zone conversion is performed and the offset is preserved.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToEndOfMonth(this DateTimeOffset dateTimeOffset)
        => dateTimeOffset.ToStartOfMonth().AddMonths(1).AddTicks(-_oneTick);

    /// <summary>
    /// Returns the start of the next month relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToStartOfNextMonth(this DateTimeOffset dateTimeOffset)
        => dateTimeOffset.ToStartOfMonth().AddMonths(1);

    /// <summary>
    /// Returns the start of the previous month relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToStartOfPreviousMonth(this DateTimeOffset dateTimeOffset)
        => dateTimeOffset.ToStartOfMonth().AddMonths(-1);

    /// <summary>
    /// Returns the end of the previous month relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <remarks>
    /// Computed as one tick before the start of the current month. No time zone conversion is performed and the offset is preserved.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToEndOfPreviousMonth(this DateTimeOffset dateTimeOffset)
        => dateTimeOffset.ToStartOfMonth().AddTicks(-_oneTick);

    /// <summary>
    /// Returns the end of the next month relative to <paramref name="dateTimeOffset"/>.
    /// </summary>
    /// <remarks>
    /// Computed as one tick before the start of the month after next. No time zone conversion is performed and the offset is preserved.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToEndOfNextMonth(this DateTimeOffset dateTimeOffset)
        => dateTimeOffset.ToStartOfMonth().AddMonths(2).AddTicks(-_oneTick);

    /// <summary>
    /// Computes the start of the month in <paramref name="tz"/> that contains the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <remarks>
    /// This computes the boundary as a local wall time (00:00 on the 1st) and maps it to UTC using the time zone's rules
    /// at that wall time (DST-safe).
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToStartOfTzMonth(this DateTimeOffset utcInstant, TimeZoneInfo tz)
        => ToStartOfTzMonthAtOffset(utcInstant, tz, 0);

    /// <summary>
    /// Computes the end of the month in <paramref name="tz"/> that contains the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <remarks>Computed as one tick before the start of the next month in <paramref name="tz"/> (DST-safe).</remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToEndOfTzMonth(this DateTimeOffset utcInstant, TimeZoneInfo tz)
        => ToStartOfTzMonthAtOffset(utcInstant, tz, 1).AddTicks(-_oneTick);

    /// <summary>
    /// Computes the start of the previous month in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToStartOfPreviousTzMonth(this DateTimeOffset utcInstant, TimeZoneInfo tz)
        => ToStartOfTzMonthAtOffset(utcInstant, tz, -1);

    /// <summary>
    /// Computes the end of the previous month in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <remarks>Computed as one tick before the start of the current month in <paramref name="tz"/> (DST-safe).</remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToEndOfPreviousTzMonth(this DateTimeOffset utcInstant, TimeZoneInfo tz)
        => ToStartOfTzMonthAtOffset(utcInstant, tz, 0).AddTicks(-_oneTick);

    /// <summary>
    /// Computes the start of the next month in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToStartOfNextTzMonth(this DateTimeOffset utcInstant, TimeZoneInfo tz)
        => ToStartOfTzMonthAtOffset(utcInstant, tz, 1);

    /// <summary>
    /// Computes the end of the next month in <paramref name="tz"/> relative to the instant <paramref name="utcInstant"/>,
    /// returning the result as a UTC <see cref="DateTimeOffset"/>.
    /// </summary>
    /// <remarks>Computed as one tick before the start of the month after next in <paramref name="tz"/> (DST-safe).</remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTimeOffset ToEndOfNextTzMonth(this DateTimeOffset utcInstant, TimeZoneInfo tz)
        => ToStartOfTzMonthAtOffset(utcInstant, tz, 2).AddTicks(-_oneTick);

    /// <summary>
    /// Returns the start of the month that is <paramref name="monthOffset"/> months from the month containing
    /// <paramref name="utcInstant"/> in <paramref name="tz"/>, as a UTC instant. Used for DST-correct TZ boundaries.
    /// </summary>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static DateTimeOffset ToStartOfTzMonthAtOffset(this DateTimeOffset utcInstant, TimeZoneInfo tz, int monthOffset)
    {
        // Normalize to UTC instant once, then work in DateTime to avoid extra DateTimeOffset conversions.
        DateTime utcDt = utcInstant.UtcDateTime;
        if (utcDt.Kind != DateTimeKind.Utc)
            utcDt = utcInstant.ToUniversalTime().UtcDateTime;

        // Local wall-clock representation in tz, used only for Year/Month selection.
        DateTime local = TimeZoneInfo.ConvertTimeFromUtc(utcDt, tz);

        // Month boundary as *local* wall time (00:00 on the 1st), then offset months.
        DateTime localStart = new(local.Year, local.Month, 1, 0, 0, 0, DateTimeKind.Unspecified);
        if (monthOffset != 0)
            localStart = localStart.AddMonths(monthOffset);

        // Map that local boundary back to UTC using tz rules (DST-safe).
        DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(localStart, tz); // Kind.Utc
        return new DateTimeOffset(utcStart, TimeSpan.Zero);
    }
}