using System;
using Core.Abstract;
using Core.Attributes;

namespace Intereceptors.ExceptionInterceptor
{
    public class ExceptionAttribute: InterceptionAttribute, IInterceptorAttribute<ExceptionAspect>
    {
    }
}