using System;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog.LogglyBulkSink.cs
{
    public static class LogglyBulkSinkExtension
    {
        /// <summary>
        ///  Create a new Loggly Bulk Sink which uses the HTTP Bulk Protocol
        /// </summary>
        /// <param name="lc">Logger Configuration</param>
        /// <param name="logglyKey">Loggly Key</param>
        /// <param name="logglyTags">Loggly Tags</param>
        /// <param name="restrictedToMinLevel">Minimum Log Level to Restrict to </param>
        /// <param name="batchPostingLimit">Batch Posting Limit, defaults to 1000</param>
        /// <param name="period">Frequency of Periodic Batch Sink auto flushing</param>
        /// <returns>Original Log Sink Configuration now updated</returns>
        /// <remarks>Depending on your aveage log event size, a batch positing limit on the order of 10000 could be reasonable</remarks>
        public static LoggerConfiguration LogglyBulk(this LoggerSinkConfiguration lc, 
            string logglyKey, 
            string[] logglyTags,
            LogEventLevel restrictedToMinLevel = LogEventLevel.Verbose,
            int batchPostingLimit = 1000,
            TimeSpan? period = null)
        {
            if (lc == null) throw new ArgumentNullException("lc");

            var frequency = period ?? TimeSpan.FromSeconds(30);

            return lc.Sink(new LogglySink(logglyKey, logglyTags, batchPostingLimit, frequency), restrictedToMinLevel);
        }
    }
}
