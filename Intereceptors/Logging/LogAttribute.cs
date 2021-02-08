﻿using System;
using Core.Abstract;
using Core.Attributes;
using Microsoft.Extensions.Logging;

namespace Intereceptors.Logging
{
    public class LogAttribute: InterceptionAttribute, IInterceptorAttribute<LogInterceptor>
    {
        internal LogLevel LogLevel { get; set; }

        public LogAttribute(LogLevel logLevel)
        {
            LogLevel = logLevel;
        }

    }
}