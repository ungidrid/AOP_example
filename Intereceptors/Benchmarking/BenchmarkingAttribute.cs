using System;
using Core.Abstract;
using Core.Attributes;

namespace Intereceptors.Benchmarking
{
    public class BenchmarkingAttribute: InterceptionAttribute, IInterceptorAttribute<BenchmarkingInterceptor>
    {
    }
}