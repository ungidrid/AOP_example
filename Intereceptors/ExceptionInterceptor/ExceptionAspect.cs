using System;
using Core;
using Core.Abstract;
using Microsoft.Extensions.Logging;

namespace Intereceptors.ExceptionInterceptor
{
    public class ExceptionAspect: AspectBase
    {
        private readonly ILogger<ExceptionAspect> _logger;

        public ExceptionAspect(ILogger<ExceptionAspect> logger)
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