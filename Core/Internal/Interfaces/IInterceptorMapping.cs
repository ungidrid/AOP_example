using System;

namespace Core.Internal.Interfaces
{
    public interface IInterceptorMapping
    {
        public Type AttributeType { get; set; }
        public Type InterceptorType { get; set; }
    }
}