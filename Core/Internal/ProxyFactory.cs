using System;
using Castle.DynamicProxy;
using Core.Configuration;

namespace Core.Internal
{
    public class ProxyFactory<T>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IProxyGenerator _proxyGenerator;
        private readonly ProxyConfiguration _configuration;

        public ProxyFactory(IServiceProvider serviceProvider, IProxyGenerator proxyGenerator, ProxyConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _proxyGenerator = proxyGenerator;
            _configuration = configuration;
        }      
        
        public T CreateProxy(T instance)
        {
            var coreInterceptor = new CoreInterceptor(_serviceProvider, _configuration);
            var proxy = _proxyGenerator.CreateInterfaceProxyWithTarget(typeof(T), instance, coreInterceptor);

            if(proxy == null)
            {
                throw new ArgumentNullException(nameof(proxy));
            }

            return (T)proxy;
        }

    }
}