using System;
using Microsoft.EntityFrameworkCore;

namespace Intereceptors.UnitOfWork
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            var options = new DbContextOptionsBuilder<TestContext>()
                .UseInMemoryDatabase("Test DB")
                .Options;

            var context = new TestContext(options);

            return new UnitOfWork(context);
        }

    }
}