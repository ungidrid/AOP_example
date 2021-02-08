using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Core.Configuration;
using Core.Internal.Extensions;

namespace Core.Internal
{
    public class CoreInterceptor : IInterceptor
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ProxyConfiguration _configuration;

        public CoreInterceptor(IServiceProvider serviceProvider, ProxyConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        public void Intercept(IInvocation invocation) => InterceptedCallDelegate(invocation)();

        private Action InterceptedCallDelegate(IInvocation invocation)
        {
            var invocationContexts =
                InvocationExtensions.GetInterceptorMetadataForMethod(invocation, _serviceProvider, _configuration);

            var reversed = invocationContexts.Reverse()
                .ToList();

            Action noActionIfBypassed = () =>
            {
                if(invocationContexts.Any(x => x.InvocationIsBypassed))
                    return;
                
                //Execute real method
                invocation.Proceed();
            };

            if(!reversed.Any())
                return noActionIfBypassed;

            Action decoratedCall = () => reversed.First()
                .Interceptor.Invoke(reversed.First(), noActionIfBypassed);

            foreach (var c in reversed.Skip(1))
            {
                var call = decoratedCall;
                decoratedCall = () => c.Interceptor.Invoke(c, call);
            }

            return decoratedCall;
        }

    }
}