using System;

namespace Core.Abstract
{
    public abstract class MethodBoundaryAspect: AspectBase
    {
        public sealed override void Invoke(InvocationContext invocationContext, Action next)
        {
            BeforeInvoke(invocationContext);

            next();

            AfterInvoke(invocationContext, invocationContext.GetMethodReturnValue());
        }

        protected abstract void BeforeInvoke(InvocationContext invocationContext);
        protected abstract void AfterInvoke(InvocationContext invocationContext, object returnValue);
    }
}