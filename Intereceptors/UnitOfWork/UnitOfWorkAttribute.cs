using System;
using Core.Abstract;
using Core.Attributes;

namespace Intereceptors.UnitOfWork
{
    public class UnitOfWorkAttribute: InterceptionAttribute, IInterceptorAttribute<UnitOfWorkAspect>
    {
        public bool SaveChanges { get; set; } = true;
    }
}