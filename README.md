[![](https://img.shields.io/nuget/v/soenneker.extensions.datetimeoffsets.months.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.datetimeoffsets.months/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.datetimeoffsets.months/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.datetimeoffsets.months/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.extensions.datetimeoffsets.months.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.extensions.datetimeoffsets.months/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.extensions.datetimeoffsets.months/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.extensions.datetimeoffsets.months/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Extensions.DateTimeOffsets.Months
A collection of helpful DateTimeOffset month extension methods.

## Installation

```bash
dotnet add package Soenneker.Extensions.DateTimeOffsets.Months
```

## Quick start

```csharp
using Soenneker.Extensions.DateTimeOffsets.Months;

DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
var result = dateTimeOffset.ToStartOfMonth();
```

## Common operations

- `ToStartOfMonth()` - Returns the start of the month containing `dateTimeOffset`. No time zone conversion is performed and the offset is preserved.
- `ToEndOfMonth()` - Returns the end of the month containing `dateTimeOffset`.
- `ToStartOfNextMonth()` - Returns the start of the next month relative to `dateTimeOffset`.
- `ToStartOfPreviousMonth()` - Returns the start of the previous month relative to `dateTimeOffset`.
- `ToEndOfPreviousMonth()` - Returns the end of the previous month relative to `dateTimeOffset`.
- `ToEndOfNextMonth()` - Returns the end of the next month relative to `dateTimeOffset`.
- `ToStartOfTzMonth()` - Computes the start of the month in `tz` that contains the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`. This computes the boundary as a local wall time (00:00 on the 1st) and maps it to UTC using the time zone's rules at that wall time (DST-safe).
- `ToEndOfTzMonth()` - Computes the end of the month in `tz` that contains the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`.
- `ToStartOfPreviousTzMonth()` - Computes the start of the previous month in `tz` relative to the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`.
- `ToEndOfPreviousTzMonth()` - Computes the end of the previous month in `tz` relative to the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`.
- `ToStartOfNextTzMonth()` - Computes the start of the next month in `tz` relative to the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`.
- `ToEndOfNextTzMonth()` - Computes the end of the next month in `tz` relative to the instant `utcInstant`, returning the result as a UTC `DateTimeOffset`.
