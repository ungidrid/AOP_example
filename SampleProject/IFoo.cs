using System;
using System.Threading.Tasks;
using Intereceptors.UnitOfWork;

namespace SampleProject
{
    public interface IFoo
    {
        void TestMethod();
        DateTime TestMethodWithExpiration();
        void AddEntity(IUnitOfWork unitOfWork = null);
        void PrintEntities(IUnitOfWork unitOfWork = null);
    }
}