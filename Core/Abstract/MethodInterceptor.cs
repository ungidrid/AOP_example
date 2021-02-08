using System;

namespace Core.Abstract
{
    public abstract class MethodInterceptor
    {
        public abstract void Invoke(InvocationContext invocationContext, Action next);
    }
}