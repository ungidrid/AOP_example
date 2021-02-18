using System;
using Castle.DynamicProxy;
using Core.Configuration;
using Core.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAspects(this IServiceCollection services, Action<ProxyConfiguration> configuration)
        {
            services.AddOptions();
            services.Configure(configuration);
            services.AddSingleton<IProxyGenerator, ProxyGenerator>();
            return services;
        }

        public static IServiceCollection AddTransientWithProxy<TInterface, TService>(this IServiceCollection services) where TService: TInterface
        {
            return services.AddWithProxyInScope<TInterface, TService>(services.AddTransient);
        }

        public static IServiceCollection AddScopedWithProxy<TInterface, TService>(this IServiceCollection services) where TService : TInterface
        {
            return services.AddWithProxyInScope<TInterface, TService>(services.AddScoped);
        }

        public static IServiceCollection AddSingletoneWithProxy<TInterface, TService>(this IServiceCollection services) where TService : TInterface
        {
            return services.AddWithProxyInScope<TInterface, TService>(services.AddSingleton);
        }

        private static IServiceCollection AddWithProxyInScope<TInterface, TService>(
            this IServiceCollection services,
            Func<Type, Func<IServiceProvider, object>, IServiceCollection> scopeMethod) where TService : TInterface
        {
            var serviceProvider = services.BuildServiceProvider();
            var configuration = services.GetProxyConfiguration();
            var proxyGenerator = serviceProvider.GetRequiredService<IProxyGenerator>();
            var proxyFactory = new ProxyFactory<TInterface>(serviceProvider, proxyGenerator, configuration);

            var instance = ActivatorUtilities.CreateInstance<TService>(serviceProvider);

            scopeMethod(typeof(TInterface), p => proxyFactory.CreateProxy(instance));

            return services;
        }

        private static ProxyConfiguration GetProxyConfiguration(this IServiceCollection services) =>
            services.BuildServiceProvider()
                .GetRequiredService<IOptions<ProxyConfiguration>>()
                .Value;
    }
}