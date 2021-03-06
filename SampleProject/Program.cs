﻿using System;
using System.Threading.Tasks;
using Core.Extensions;
using Intereceptors.Benchmarking;
using Intereceptors.Caching;
using Intereceptors.ExceptionInterceptor;
using Intereceptors.Logging;
using Intereceptors.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace SampleProject
{
    class Program
    {
        public static ServiceProvider ServiceProvider { get; set; }

        static Program()
        {
            ConfigureServices();
        }

        static async Task Main(string[] args)
        {
            var testProxy = ServiceProvider.GetService<IFoo>();
            //Example 1
            // testProxy.TestMethod();

            //Example 2
            // var time1 = testProxy.TestMethodWithExpiration();
            // var time2 = testProxy.TestMethodWithExpiration();
            // await Task.Delay(10000);
            // var time3 = testProxy.TestMethodWithExpiration();
            // Console.WriteLine($"Time 1: {time1.TimeOfDay}, Time 2: {time2.TimeOfDay}, Time 3: {time3.TimeOfDay}");
            
            //
            // //Example 3
            testProxy.AddEntity();
            testProxy.PrintEntities();
        }

        static void ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddMemoryCache();
            services.AddLogging(
                c => c.AddConsole()
                    .SetMinimumLevel(LogLevel.Trace));

            services.AddAspects(
                x => x.AddAspect<LogAttribute, LogAspect>()
                    .AddAspect<BenchmarkingAttribute, BenchmarkingAspect>()
                    .AddAspect<CacheAttribute, CacheAspect>()
                    .AddAspect<ExceptionAttribute, ExceptionAspect>()
                    .AddAspect<UnitOfWorkAttribute, UnitOfWorkAspect>());

            services.AddTransient<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddTransientWithProxy<IFoo, Foo>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
