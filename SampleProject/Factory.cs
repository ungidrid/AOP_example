using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace SampleProject
{
    public interface IDependency
    {
    }

    public class Model
    {
        private readonly int _param;
        private readonly IDependency _dependency;

        public Model(int param, IDependency dependency)
        {
            _param = param;
            _dependency = dependency;
        }
    }

    public interface IModelFactory
    {
        Model CreateModel(int param);
    }

    public class ModelFactory: IModelFactory
    {
        private readonly IDependency _dependency;

        public ModelFactory(IDependency dependency)
        {
            _dependency = dependency;
        }

        public Model CreateModel(int param)
        {
            return new Model(param, _dependency);
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoFactory<TFactory>(this IServiceCollection services)
            where TFactory : class
        {
            services.AddSingleton<TFactory>(CreateFactory<TFactory>);
            return services;
        }

        private static TFactory CreateFactory<TFactory>(IServiceProvider serviceProvider)
            where TFactory : class
        {
            var generator = new ProxyGenerator();
            return generator.CreateInterfaceProxyWithoutTarget<TFactory>(
                new FactoryInterceptor(serviceProvider));
        }

        private class FactoryInterceptor : IInterceptor
        {
            private readonly IServiceProvider _serviceProvider;

            public FactoryInterceptor(IServiceProvider serviceProvider)
            {
                _serviceProvider = serviceProvider;
            }

            public void Intercept(IInvocation invocation)
            {
                var factory = CreateFactory(invocation.Method);
                invocation.ReturnValue = factory(_serviceProvider, invocation.Arguments);
            }

            private ObjectFactory CreateFactory(MethodInfo method)
            {
                return ActivatorUtilities.CreateFactory(
                    method.ReturnType,
                    method.GetParameters().Select(p => p.ParameterType).ToArray());
            }
        }
    }
}