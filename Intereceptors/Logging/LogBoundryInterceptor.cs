using System;
using Core;
using Core.Abstract;
using Microsoft.Extensions.Logging;

namespace Intereceptors.Logging
{
    public class LogInterceptor: MethodBoundaryInterceptor
    {
        private readonly ILoggerFactory _loggerFactory;
        private ILogger _logger;
        private LogAttribute _logAttribute;

        public LogInterceptor(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }


        protected override void BeforeInvoke(InvocationContext invocationContext)
        {
            _logger = _loggerFactory.CreateLogger(invocationContext.GetOwningType());
            _logAttribute = invocationContext.GetOwningAttribute() as LogAttribute;

            var level = _logAttribute.LogLevel;

            _logger.Log(level, "{owningType}: Method executing: {methodName}", invocationContext.GetOwningType(), invocationContext.GetExecutingMethodName());
        }

        protected override void AfterInvoke(InvocationContext invocationContext, object methodResult)
        {
            _logger.Log(_logAttribute.LogLevel, "{owningType}: Method executed: {methodName}", invocationContext.GetOwningType(), invocationContext.GetExecutingMethodName());
        }
    }
}