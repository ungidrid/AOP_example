using System;
using Core;
using Core.Abstract;
using Microsoft.Extensions.Logging;

namespace Intereceptors.ExceptionInterceptor
{
    public class ExceptionInterceptor: MethodInterceptor
    {
        private readonly ILogger<ExceptionInterceptor> _logger;

        public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        public override void Invoke(InvocationContext invocationContext, Action next) 
        {
            try
            {
                next();
            }
            catch
            {
                _logger.LogError("Exception caught in {methodName}", invocationContext.GetExecutingMethodName());
            }
        }
    }
}