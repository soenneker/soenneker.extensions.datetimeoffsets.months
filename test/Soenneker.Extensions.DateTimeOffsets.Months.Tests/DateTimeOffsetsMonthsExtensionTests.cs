using System;
using AwesomeAssertions;
using Soenneker.Tests.Unit;
using Xunit;

namespace Soenneker.Extensions.DateTimeOffsets.Months.Tests;

public sealed class DateTimeOffsetsMonthsExtensionTests : UnitTest
{
    private static readonly TimeSpan MinusFive = TimeSpan.FromHours(-5);
    private static readonly DateTimeOffset MidMarch2024 = new(2024, 3, 15, 14, 30, 0, MinusFive);

    [Fact]
    public void ToStartOfMonth_returns_first_moment_of_same_month_and_preserves_offset()
    {
        DateTimeOffset result = MidMarch2024.ToStartOfMonth();

        result.Year.Should().Be(2024);
        result.Month.Should().Be(3);
        result.Day.Should().Be(1);
        result.Hour.Should().Be(0);
        result.Minute.Should().Be(0);
        result.Second.Should().Be(0);
        result.Offset.Should().Be(MinusFive);
    }

    [Fact]
    public void ToEndOfMonth_returns_last_tick_of_same_month()
    {
        DateTimeOffset result = MidMarch2024.ToEndOfMonth();

        result.Year.Should().Be(2024);
        result.Month.Should().Be(3);
        result.Day.Should().Be(31);
        result.Hour.Should().Be(23);
        result.Minute.Should().Be(59);
        result.Second.Should().Be(59);
        result.Offset.Should().Be(MinusFive);
        result.AddTicks(1).Month.Should().Be(4);
    }

    [Fact]
    public void ToStartOfNextMonth_returns_first_moment_of_next_month()
    {
        DateTimeOffset result = MidMarch2024.ToStartOfNextMonth();

        result.Year.Should().Be(2024);
        result.Month.Should().Be(4);
        result.Day.Should().Be(1);
        result.Hour.Should().Be(0);
        result.Minute.Should().Be(0);
        result.Second.Should().Be(0);
        result.Offset.Should().Be(MinusFive);
    }

    [Fact]
    public void ToStartOfPreviousMonth_returns_first_moment_of_previous_month()
    {
        DateTimeOffset result = MidMarch2024.ToStartOfPreviousMonth();

        result.Year.Should().Be(2024);
        result.Month.Should().Be(2);
        result.Day.Should().Be(1);
        result.Hour.Should().Be(0);
        result.Minute.Should().Be(0);
        result.Second.Should().Be(0);
        result.Offset.Should().Be(MinusFive);
    }

    [Fact]
    public void ToEndOfPreviousMonth_returns_last_tick_before_current_month()
    {
        DateTimeOffset result = MidMarch2024.ToEndOfPreviousMonth();

        result.Year.Should().Be(2024);
        result.Month.Should().Be(2);
        result.Day.Should().Be(29); // 2024 is leap year
        result.Hour.Should().Be(23);
        result.Minute.Should().Be(59);
        result.Second.Should().Be(59);
        result.Offset.Should().Be(MinusFive);
        result.AddTicks(1).Month.Should().Be(3);
    }

    [Fact]
    public void ToEndOfNextMonth_returns_last_tick_of_next_month()
    {
        DateTimeOffset result = MidMarch2024.ToEndOfNextMonth();

        result.Year.Should().Be(2024);
        result.Month.Should().Be(4);
        result.Day.Should().Be(30);
        result.Hour.Should().Be(23);
        result.Minute.Should().Be(59);
        result.Second.Should().Be(59);
        result.Offset.Should().Be(MinusFive);
        result.AddTicks(1).Month.Should().Be(5);
    }

    [Fact]
    public void ToStartOfTzMonth_returns_utc_start_of_month_in_specified_time_zone()
    {
        // 2024-03-15 19:30 UTC = 2024-03-15 14:30 -05:00
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");

        DateTimeOffset result = utcInstant.ToStartOfTzMonth(tz);

        result.Offset.Should().Be(TimeSpan.Zero);
        result.UtcDateTime.Year.Should().Be(2024);
        result.UtcDateTime.Month.Should().Be(3);
        result.UtcDateTime.Day.Should().Be(1);
        result.UtcDateTime.Hour.Should().Be(5); // 00:00 EST = 05:00 UTC (before DST)
        result.UtcDateTime.Minute.Should().Be(0);
        result.UtcDateTime.Second.Should().Be(0);
    }

    [Fact]
    public void ToEndOfTzMonth_returns_utc_end_of_month_in_specified_time_zone()
    {
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");

        DateTimeOffset result = utcInstant.ToEndOfTzMonth(tz);

        result.Offset.Should().Be(TimeSpan.Zero);
        // End of March in TZ is one tick before April 1 in that TZ; in UTC it may be April 1
        result.AddTicks(1).Month.Should().Be(4);
        result.AddTicks(1).Year.Should().Be(2024);
    }

    [Fact]
    public void ToStartOfPreviousTzMonth_returns_utc_start_of_previous_month_in_tz()
    {
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");

        DateTimeOffset result = utcInstant.ToStartOfPreviousTzMonth(tz);

        result.Offset.Should().Be(TimeSpan.Zero);
        result.UtcDateTime.Year.Should().Be(2024);
        result.UtcDateTime.Month.Should().Be(2);
        result.UtcDateTime.Day.Should().Be(1);
        result.UtcDateTime.Hour.Should().Be(5);
        result.UtcDateTime.Minute.Should().Be(0);
        result.UtcDateTime.Second.Should().Be(0);
    }

    [Fact]
    public void ToEndOfPreviousTzMonth_returns_utc_end_of_previous_month_in_tz()
    {
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");

        DateTimeOffset result = utcInstant.ToEndOfPreviousTzMonth(tz);

        result.Offset.Should().Be(TimeSpan.Zero);
        // End of Feb in TZ is one tick before Mar 1 in that TZ; in UTC it may be Mar 1
        result.AddTicks(1).Month.Should().Be(3);
        result.AddTicks(1).Year.Should().Be(2024);
    }

    [Fact]
    public void ToStartOfNextTzMonth_returns_utc_start_of_next_month_in_tz()
    {
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");

        DateTimeOffset result = utcInstant.ToStartOfNextTzMonth(tz);

        result.Offset.Should().Be(TimeSpan.Zero);
        result.UtcDateTime.Year.Should().Be(2024);
        result.UtcDateTime.Month.Should().Be(4);
        result.UtcDateTime.Day.Should().Be(1);
    }

    [Fact]
    public void ToEndOfNextTzMonth_returns_utc_end_of_next_month_in_tz()
    {
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");

        DateTimeOffset result = utcInstant.ToEndOfNextTzMonth(tz);

        result.Offset.Should().Be(TimeSpan.Zero);
        // End of April in TZ is one tick before May 1 in that TZ; in UTC it may be May 1
        result.AddTicks(1).Month.Should().Be(5);
        result.AddTicks(1).Year.Should().Be(2024);
    }

    [Fact]
    public void ToStartOfMonth_on_first_day_preserves_value()
    {
        var firstOfMonth = new DateTimeOffset(2024, 6, 1, 0, 0, 0, TimeSpan.Zero);
        DateTimeOffset result = firstOfMonth.ToStartOfMonth();

        result.Should().Be(firstOfMonth);
    }

    [Fact]
    public void ToStartOfMonth_on_last_day_of_month_returns_same_month_start()
    {
        var lastOfMonth = new DateTimeOffset(2024, 6, 30, 23, 59, 59, TimeSpan.Zero);
        DateTimeOffset result = lastOfMonth.ToStartOfMonth();

        result.Year.Should().Be(2024);
        result.Month.Should().Be(6);
        result.Day.Should().Be(1);
        result.Hour.Should().Be(0);
        result.Minute.Should().Be(0);
        result.Second.Should().Be(0);
    }

    // --- Weird / edge scenarios ---

    [Fact]
    public void ToStartOfNextMonth_December_returns_January_next_year()
    {
        var dec15 = new DateTimeOffset(2024, 12, 15, 12, 0, 0, TimeSpan.Zero);
        DateTimeOffset result = dec15.ToStartOfNextMonth();

        result.Year.Should().Be(2025);
        result.Month.Should().Be(1);
        result.Day.Should().Be(1);
        result.Hour.Should().Be(0);
        result.Minute.Should().Be(0);
        result.Second.Should().Be(0);
    }

    [Fact]
    public void ToStartOfPreviousMonth_January_returns_December_previous_year()
    {
        var jan15 = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero);
        DateTimeOffset result = jan15.ToStartOfPreviousMonth();

        result.Year.Should().Be(2023);
        result.Month.Should().Be(12);
        result.Day.Should().Be(1);
    }

    [Fact]
    public void ToEndOfMonth_February_leap_year_returns_29th()
    {
        var midFeb = new DateTimeOffset(2024, 2, 15, 12, 0, 0, TimeSpan.Zero);
        DateTimeOffset result = midFeb.ToEndOfMonth();

        result.Year.Should().Be(2024);
        result.Month.Should().Be(2);
        result.Day.Should().Be(29);
        result.Hour.Should().Be(23);
        result.Minute.Should().Be(59);
        result.Second.Should().Be(59);
        result.AddTicks(1).Month.Should().Be(3);
    }

    [Fact]
    public void ToEndOfMonth_February_non_leap_year_returns_28th()
    {
        var midFeb = new DateTimeOffset(2023, 2, 15, 12, 0, 0, TimeSpan.Zero);
        DateTimeOffset result = midFeb.ToEndOfMonth();

        result.Year.Should().Be(2023);
        result.Month.Should().Be(2);
        result.Day.Should().Be(28);
        result.AddTicks(1).Month.Should().Be(3);
    }

    [Fact]
    public void ToEndOfMonth_30_day_month_returns_30th()
    {
        var midApril = new DateTimeOffset(2024, 4, 15, 12, 0, 0, TimeSpan.Zero);
        DateTimeOffset result = midApril.ToEndOfMonth();

        result.Month.Should().Be(4);
        result.Day.Should().Be(30);
        result.AddTicks(1).Month.Should().Be(5);
    }

    [Fact]
    public void ToEndOfNextMonth_December_returns_end_of_January_next_year()
    {
        var dec15 = new DateTimeOffset(2024, 12, 15, 12, 0, 0, TimeSpan.Zero);
        DateTimeOffset result = dec15.ToEndOfNextMonth();

        result.Year.Should().Be(2025);
        result.Month.Should().Be(1);
        result.Day.Should().Be(31);
        result.AddTicks(1).Month.Should().Be(2);
    }

    [Fact]
    public void ToEndOfPreviousMonth_January_returns_end_of_December_previous_year()
    {
        var jan15 = new DateTimeOffset(2024, 1, 15, 12, 0, 0, TimeSpan.Zero);
        DateTimeOffset result = jan15.ToEndOfPreviousMonth();

        result.Year.Should().Be(2023);
        result.Month.Should().Be(12);
        result.Day.Should().Be(31);
        result.AddTicks(1).Month.Should().Be(1);
        result.AddTicks(1).Year.Should().Be(2024);
    }

    [Fact]
    public void ToStartOfTzMonth_null_timezone_throws()
    {
        var utc = new DateTimeOffset(2024, 3, 15, 12, 0, 0, TimeSpan.Zero);
        Action act = () => utc.ToStartOfTzMonth(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToEndOfTzMonth_null_timezone_throws()
    {
        var utc = new DateTimeOffset(2024, 3, 15, 12, 0, 0, TimeSpan.Zero);
        Action act = () => utc.ToEndOfTzMonth(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Offset_preserved_through_all_non_tz_methods()
    {
        var withOffset = new DateTimeOffset(2024, 7, 4, 14, 30, 0, TimeSpan.FromHours(9));
        TimeSpan offset = withOffset.Offset;

        withOffset.ToStartOfMonth().Offset.Should().Be(offset);
        withOffset.ToEndOfMonth().Offset.Should().Be(offset);
        withOffset.ToStartOfNextMonth().Offset.Should().Be(offset);
        withOffset.ToStartOfPreviousMonth().Offset.Should().Be(offset);
        withOffset.ToEndOfPreviousMonth().Offset.Should().Be(offset);
        withOffset.ToEndOfNextMonth().Offset.Should().Be(offset);
    }

    /// <summary>
    /// End of month in a DST timezone should be "last tick of last day in that zone" in UTC.
    /// The implementation uses AddMonths(1) on a UTC value, which does not account for DST:
    /// "March 1 00:00 Eastern" = 05:00 UTC (EST). AddMonths(1) => "April 1 05:00 UTC".
    /// So ToEndOfTzMonth yields April 1 04:59:59.9999999 UTC.
    /// Correct "end of March in Eastern" is March 31 23:59:59.9999999 EDT = April 1 03:59:59.9999999 UTC.
    /// This test documents the expected correct behavior; it may fail if the extension has the DST bug.
    /// </summary>
    [Fact]
    public void ToEndOfTzMonth_DST_should_equal_last_tick_of_month_in_timezone()
    {
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");
        // March 2024: instant in the middle of the month
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);

        // Correct end of March in Eastern: March 31 23:59:59.9999999 local → UTC
        var lastSecondOfDay = new DateTime(2024, 3, 31, 23, 59, 59, DateTimeKind.Unspecified);
        var lastTickLocal = lastSecondOfDay.AddTicks(9999999); // last tick of that second
        DateTime expectedUtcEnd = TimeZoneInfo.ConvertTimeToUtc(lastTickLocal, tz);
        var expected = new DateTimeOffset(expectedUtcEnd, TimeSpan.Zero);

        DateTimeOffset actual = utcInstant.ToEndOfTzMonth(tz);

        actual.UtcDateTime.Should().Be(expected.UtcDateTime,
            "ToEndOfTzMonth should return the last tick of the month in the given time zone (DST-safe). " +
            "If this fails, the implementation may be using AddMonths(1) on UTC instead of computing next month start in TZ.");
    }

    /// <summary>
    /// Start of next month in a DST timezone must be "first moment of next month in that zone" in UTC.
    /// March 1 00:00 EST = 05:00 UTC. April 1 00:00 EDT = 04:00 UTC.
    /// So ToStartOfNextTzMonth(instant in March) should return April 1 04:00 UTC, not April 1 05:00 UTC.
    /// </summary>
    [Fact]
    public void ToStartOfNextTzMonth_DST_should_equal_first_tick_of_next_month_in_timezone()
    {
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);

        var april1Local = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Unspecified);
        DateTime expectedUtc = TimeZoneInfo.ConvertTimeToUtc(april1Local, tz);
        var expected = new DateTimeOffset(expectedUtc, TimeSpan.Zero);

        DateTimeOffset actual = utcInstant.ToStartOfNextTzMonth(tz);

        actual.UtcDateTime.Should().Be(expected.UtcDateTime,
            "ToStartOfNextTzMonth should return the first moment of the next month in the given time zone (DST-safe). " +
            "If this fails, the implementation may be using AddMonths(1) on UTC instead of computing next month in TZ.");
    }

    /// <summary>
    /// ToEndOfNextTzMonth: end of April in Eastern. April 30 23:59:59.9999999 EDT → UTC.
    /// Implementation does ToStartOfTzMonth + AddMonths(2) + AddTicks(-1). That adds 2 months in UTC to
    /// "March 1 00:00 Eastern" = March 1 05:00 UTC → May 1 05:00 UTC, -1 tick = May 1 04:59:59.9999999 UTC.
    /// Correct: April 30 23:59:59.9999999 EDT = May 1 03:59:59.9999999 UTC.
    /// </summary>
    [Fact]
    public void ToEndOfNextTzMonth_DST_should_equal_last_tick_of_next_month_in_timezone()
    {
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);

        var lastSecondApril30 = new DateTime(2024, 4, 30, 23, 59, 59, DateTimeKind.Unspecified);
        var lastTickLocal = lastSecondApril30.AddTicks(9999999);
        DateTime expectedUtc = TimeZoneInfo.ConvertTimeToUtc(lastTickLocal, tz);
        var expected = new DateTimeOffset(expectedUtc, TimeSpan.Zero);

        DateTimeOffset actual = utcInstant.ToEndOfNextTzMonth(tz);

        actual.UtcDateTime.Should().Be(expected.UtcDateTime,
            "ToEndOfNextTzMonth should return the last tick of the next month in the given time zone (DST-safe).");
    }

    /// <summary>
    /// ToEndOfPreviousTzMonth: end of February in Eastern. Feb 29 23:59:59.9999999 EST (leap 2024) → UTC.
    /// Implementation: ToStartOfTzMonth (March 1 00:00 Eastern) + AddTicks(-1) = March 1 05:00 UTC - 1 tick = March 1 04:59:59.9999999 UTC.
    /// In Eastern that's Feb 29 23:59:59.9999999 — correct! So ToEndOfPreviousTzMonth does NOT use AddMonths, it's correct.
    /// But we need to ensure the instant is right: Feb 29 23:59:59.9999999 EST = March 1 04:59:59.9999999 UTC.
    /// </summary>
    [Fact]
    public void ToEndOfPreviousTzMonth_DST_should_equal_last_tick_of_previous_month_in_timezone()
    {
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);

        var lastSecondFeb29 = new DateTime(2024, 2, 29, 23, 59, 59, DateTimeKind.Unspecified);
        var lastTickLocal = lastSecondFeb29.AddTicks(9999999);
        DateTime expectedUtc = TimeZoneInfo.ConvertTimeToUtc(lastTickLocal, tz);
        var expected = new DateTimeOffset(expectedUtc, TimeSpan.Zero);

        DateTimeOffset actual = utcInstant.ToEndOfPreviousTzMonth(tz);

        actual.UtcDateTime.Should().Be(expected.UtcDateTime,
            "ToEndOfPreviousTzMonth should return the last tick of the previous month in the given time zone.");
    }

    /// <summary>
    /// ToStartOfPreviousTzMonth: start of February in Eastern. Feb 1 00:00 EST = Feb 1 05:00 UTC.
    /// Implementation: ToStartOfTzMonth (March 1 00:00) + AddMonths(-1) = March 1 05:00 UTC - 1 month = Feb 1 05:00 UTC. Correct for EST.
    /// So this one might pass; the bug is only when the *next* month has different DST (AddMonths(1) or AddMonths(2)).
    /// </summary>
    [Fact]
    public void ToStartOfPreviousTzMonth_returns_first_tick_of_previous_month_in_timezone()
    {
        TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(
            OperatingSystem.IsWindows() ? "Eastern Standard Time" : "America/New_York");
        var utcInstant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);

        var feb1Local = new DateTime(2024, 2, 1, 0, 0, 0, DateTimeKind.Unspecified);
        DateTime expectedUtc = TimeZoneInfo.ConvertTimeToUtc(feb1Local, tz);
        var expected = new DateTimeOffset(expectedUtc, TimeSpan.Zero);

        DateTimeOffset actual = utcInstant.ToStartOfPreviousTzMonth(tz);

        actual.UtcDateTime.Should().Be(expected.UtcDateTime);
    }
}
