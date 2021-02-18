using System;

namespace Core.Abstract
{
    public abstract class AspectBase
    {
        public abstract void Invoke(InvocationContext invocationContext, Action next);
    }
}