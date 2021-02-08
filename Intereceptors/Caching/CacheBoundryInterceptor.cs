using System;
using Core;
using Core.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace Intereceptors.Caching
{
    public class CacheInterceptor: MethodBoundaryInterceptor
    {
        private readonly IMemoryCache _memoryCache;

        public CacheInterceptor(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        protected override void BeforeInvoke(InvocationContext invocationContext)
        {
            _memoryCache.TryGetValue(invocationContext.GetExecutingMethodName(), out var result);

            if(result != null)
            {
                invocationContext.OverrideMethodReturnValue(result);
                invocationContext.BypassInvocation();
            }
        }

        protected override void AfterInvoke(InvocationContext invocationContext, object returnValue)
        {
            var expirationTime = (invocationContext.GetOwningAttribute() as CacheAttribute)
                .ExpireIn;
            if(expirationTime > 0)
            {
                _memoryCache.Set(invocationContext.GetExecutingMethodName(), returnValue, TimeSpan.FromMilliseconds(expirationTime));
            }
            else
            {
                _memoryCache.Set(invocationContext.GetExecutingMethodName(), returnValue);
            }
        }
    }
}