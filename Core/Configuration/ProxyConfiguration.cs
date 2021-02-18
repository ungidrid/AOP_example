using System.Collections.Generic;
using Core.Abstract;
using Core.Internal.Configuration;
using Core.Internal.Interfaces;

namespace Core.Configuration
{
    public class ProxyConfiguration
    {
        public List<IInterceptorMapping> ConfiguredInterceptors { get; set; } = new List<IInterceptorMapping>();

        public ProxyConfiguration AddAspect<TAttribute, TInterceptor>() where TAttribute : IInterceptorAttribute<TInterceptor>
                                                                             where TInterceptor : AspectBase
        {
            ConfiguredInterceptors.Add(new InterceptorMapping<TAttribute, TInterceptor>());
            return this;
        }
    }
}