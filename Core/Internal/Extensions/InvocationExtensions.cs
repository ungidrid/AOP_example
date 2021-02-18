using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Core.Abstract;
using Core.Attributes;
using Core.Configuration;
using Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Internal.Extensions
{
    static class InvocationExtensions
    {
        internal static IEnumerable<InvocationContext> GetInterceptorMetadataForMethod(
            IInvocation invocation,
            IServiceProvider serviceProvider,
            ProxyConfiguration proxyConfiguration)
        {
            var interceptorList = new List<InvocationContext>();

            var classAttributes = FilterInterceptionAttributes(invocation.InvocationTarget.GetType().GetCustomAttributes());
            var methodAttributes = FilterInterceptionAttributes(invocation.MethodInvocationTarget.GetCustomAttributes());
            var attribtutes = classAttributes.Concat(methodAttributes)
                .ToList();

            var clearAttributeIndex = attribtutes.FindLastIndex(x => x is ClearInterceptorsAttribute);
            if(clearAttributeIndex != -1)
            {
                attribtutes = attribtutes
                    .Skip(clearAttributeIndex + 1)
                    .ToList();
            }


            /*
             * If same attribute is applied to a class and a method,
             * we check if it allows multiple and either take both
             * or the one, applied to the method. Double reverse is
             * required to keep correct interceptors order.
             */
            var finalInterceptors = attribtutes.AsEnumerable()
                .Cast<InterceptionAttribute>()
                .Reverse()
                .GroupBy(x => x.GetType())
                .SelectMany(
                    x => x.Key.GetCustomAttribute<AttributeUsageAttribute>()
                             ?.AllowMultiple
                         ?? false
                             ? x.ToArray()
                             : new[] {x.First()})
                .Reverse()
                .ToList();


            foreach (var interceptionAttribute in finalInterceptors)
            {
                var interceptorType = proxyConfiguration.ConfiguredInterceptors
                    .FirstOrDefault(x => x.AttributeType == interceptionAttribute.GetType())
                    ?.InterceptorType;

                if (interceptorType == null)
                {
                    throw new InvalidInterceptorException(
                        $"The Interceptor Attribute '{interceptionAttribute}' is applied to the method, but there is no configured interceptor to handle it");
                }

                var instance = (AspectBase)ActivatorUtilities.CreateInstance(serviceProvider, interceptorType);

                var context = new InvocationContext
                {
                    Attribute = interceptionAttribute,
                    Interceptor = instance,
                    Invocation = invocation,
                    ServiceProvider = serviceProvider
                };

                interceptorList.Add(context);
            }

            return interceptorList;
        }

        private static IEnumerable<Attribute> FilterInterceptionAttributes(IEnumerable<Attribute> attributes)
        {
            return attributes
                .Where(
                    x => x.GetType().IsSubclassOf(typeof(InterceptionAttribute)) 
                        || x is ClearInterceptorsAttribute)
                .ToArray();
        }
}
}