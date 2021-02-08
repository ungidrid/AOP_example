using System;
using Core.Abstract;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public abstract class InterceptionAttribute: Attribute 
    {
    }
}