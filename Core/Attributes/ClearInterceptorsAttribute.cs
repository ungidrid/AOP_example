using System;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ClearInterceptorsAttribute: Attribute
    {
    }
}