using System.Collections.Generic;
using Core.Abstract;
using Core.Internal.Configuration;
using Core.Internal.Interfaces;

namespace Core.Configuration
{
    public class ProxyConfiguration
    {
        public List<IInterceptorMapping> ConfiguredInterceptors { get; set; } = new List<IInterceptorMapping>();

        public ProxyConfiguration AddInterceptor<TAttribute, TInterceptor>() where TAttribute : IInterceptorAttribute<TInterceptor>
                                                                             where TInterceptor : MethodInterceptor
        {
            ConfiguredInterceptors.Add(new InterceptorMapping<TAttribute, TInterceptor>());
            return this;
        }
    }
}