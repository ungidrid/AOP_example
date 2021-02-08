using System;
using Core.Internal.Interfaces;

namespace Core.Internal.Configuration
{
    public class InterceptorMapping<TAttribute, TInterceptor>: IInterceptorMapping
    {
        public Type AttributeType { get; set; }
        public Type InterceptorType { get; set; }

        public InterceptorMapping()
        {
            AttributeType = typeof(TAttribute);
            InterceptorType = typeof(TInterceptor);
        }
    }
}