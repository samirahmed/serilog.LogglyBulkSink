# serlog.logglybulksink

A Bulk Http Loggly Sink for [Serilog](https://github.com/serilog/serilog)
That supports upto 10000x more efficient logging to loggly by making use of the loggly bulk http endpoint.

## Setup

Install via nuget
```
Install-Package Serilog.LogglyBulkSink
```

## Usage

```csharp
 var logglyKey = "125125125125125125125125" // whatever you loggly key is
 var tags = new []{"PROD"}
 var logger = new LoggerConfiguration()
      .MinimumLevel.Verbose()
      .WriteTo.Loggly(logglyKey, new[] {sourceTag}, batchPostingLimit: 10000, period: TimeSpan.FromSeconds(10))
      .CreateLogger();

```

