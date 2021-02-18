using Core.Abstract;
using Core.Attributes;

namespace Intereceptors.Caching
{
    public class CacheAttribute: InterceptionAttribute, IInterceptorAttribute<CacheAspect>
    {
        public int ExpireIn { get; set; }
    }
}