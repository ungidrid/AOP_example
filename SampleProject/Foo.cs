using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Core.Attributes;
using Intereceptors.Benchmarking;
using Intereceptors.Caching;
using Intereceptors.ExceptionInterceptor;
using Intereceptors.Logging;
using Intereceptors.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace SampleProject
{
    [Log(LogLevel.Trace)]
    [Benchmarking]
    public class Foo: IFoo
    {
        private readonly ILogger<Foo> _logger;

        public Foo(ILogger<Foo> logger)
        {
            _logger = logger;
        }


        [Benchmarking]
        [Exception]
        public void TestMethod() 
        {
            _logger.LogInformation("\t===> The Real Method is Executed Here! <===");
            throw new Exception();
        }

        [ClearInterceptors]
        [Cache(ExpireIn = 9000)]
        public DateTime TestMethodWithExpiration() 
        {
            _logger.LogInformation("\t===> The Real Method With Expiration is Executed Here! <===");
            Thread.Sleep(1000);
            return DateTime.Now;
        }

        [ClearInterceptors]
        [UnitOfWork]
        public void AddEntity(IUnitOfWork unitOfWork = null)
        {
            var entities = new [] { 
                new Entity{Name = "MyEntity1"},
                new Entity{Name = "MyEntity2"},
                new Entity{Name = "MyEntity3"},
                new Entity{Name = "MyEntity4"}
            };

            entities.ToList().ForEach(unitOfWork.Entities.Add);
        }

        [ClearInterceptors]
        [UnitOfWork(SaveChanges = false)]
        public void PrintEntities(IUnitOfWork unitOfWork = null)
        {
            foreach(var entity in unitOfWork.Entities.GetAll())
            {
                _logger.LogInformation("\tEntityName: {entityName}", entity);
                Thread.Sleep(10);
            }
        }
    }
}