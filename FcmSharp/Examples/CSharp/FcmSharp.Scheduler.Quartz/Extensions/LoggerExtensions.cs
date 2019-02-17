// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace FcmSharp.Scheduler.Quartz.Extensions
{
    public static class LoggerExtensions
    {
        public static bool IsDebugEnabled<TLoggerType>(this ILogger<TLoggerType> logger)
        {
            return logger.IsEnabled(LogLevel.Debug);
        }

        public static bool IsCriticalEnabled<TLoggerType>(this ILogger<TLoggerType> logger)
        {
            return logger.IsEnabled(LogLevel.Critical);
        }

        public static bool IsErrorEnabled<TLoggerType>(this ILogger<TLoggerType> logger)
        {
            return logger.IsEnabled(LogLevel.Error);
        }

        public static bool IsInformationEnabled<TLoggerType>(this ILogger<TLoggerType> logger)
        {
            return logger.IsEnabled(LogLevel.Information);
        }

        public static bool IsTraceEnabled<TLoggerType>(this ILogger<TLoggerType> logger)
        {
            return logger.IsEnabled(LogLevel.Trace);
        }

        public static bool IsWarningEnabled<TLoggerType>(this ILogger<TLoggerType> logger)
        {
            return logger.IsEnabled(LogLevel.Warning);
        }
    }
}
