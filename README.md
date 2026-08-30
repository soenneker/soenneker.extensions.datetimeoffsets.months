[![](https://img.shields.io/nuget/v/soenneker.extensions.datetimeoffsets.months.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.datetimeoffsets.months/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.datetimeoffsets.months/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.datetimeoffsets.months/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.datetimeoffsets.months.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.datetimeoffsets.months/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.datetimeoffsets.months/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.datetimeoffsets.months/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.DateTimeOffsets.Months

Month-boundary extensions for `DateTimeOffset`, including time-zone-aware boundaries returned as UTC instants.

## Installation

```bash
dotnet add package Soenneker.Extensions.DateTimeOffsets.Months
```

## Offset-preserving boundaries

Use these methods when the value's existing calendar month and offset are already the frame of reference. They do not apply time-zone rules.

```csharp
using Soenneker.Extensions.DateTimeOffsets.Months;

var value = new DateTimeOffset(2024, 3, 15, 14, 30, 0, TimeSpan.FromHours(-5));

DateTimeOffset start = value.ToStartOfMonth();
// 2024-03-01 00:00:00 -05:00

DateTimeOffset end = value.ToEndOfMonth();
// 2024-03-31 23:59:59.9999999 -05:00
```

Available operations are `ToStartOfMonth()`, `ToEndOfMonth()`, `ToStartOfPreviousMonth()`, `ToEndOfPreviousMonth()`, `ToStartOfNextMonth()`, and `ToEndOfNextMonth()`.

## Time-zone month boundaries

Use the `Tz` methods when the input represents an instant and the month must be determined in a particular time zone. The result always has a zero offset.

```csharp
TimeZoneInfo eastern = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
var instant = new DateTimeOffset(2024, 3, 15, 19, 30, 0, TimeSpan.Zero);

DateTimeOffset startUtc = instant.ToStartOfTzMonth(eastern);
// 2024-03-01 05:00:00 +00:00

DateTimeOffset endUtc = instant.ToEndOfTzMonth(eastern);
// One tick before 2024-04-01 04:00:00 +00:00
```

The time-zone methods are `ToStartOfTzMonth()`, `ToEndOfTzMonth()`, `ToStartOfPreviousTzMonth()`, `ToEndOfPreviousTzMonth()`, `ToStartOfNextTzMonth()`, and `ToEndOfNextTzMonth()`.

End methods are inclusive: they return one tick before the following month begins. Boundaries follow the supplied time zone's transition rules; if a historical transition skipped local midnight, the start resolves to the first valid local time in that month.
