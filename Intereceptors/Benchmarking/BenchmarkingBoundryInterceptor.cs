using System.Diagnostics;
using Core;
using Core.Abstract;
using Microsoft.Extensions.Logging;

namespace Intereceptors.Benchmarking
{
    public class BenchmarkingInterceptor: MethodBoundaryInterceptor
    {
        private readonly ILogger _logger;
        private readonly Stopwatch _stopWatch;

        public BenchmarkingInterceptor(ILogger<BenchmarkingInterceptor> logger)
        {
            _stopWatch = new Stopwatch();
            _logger = logger;
        }

        protected override void BeforeInvoke(InvocationContext invocationContext) 
        {
            _stopWatch.Start();
        }

        protected override void AfterInvoke(InvocationContext invocationContext, object returnValue) 
        {
            _stopWatch.Stop();
            _logger.LogInformation("Method executed in: {milliseconds}ms", _stopWatch.ElapsedMilliseconds);
        }
    }
}