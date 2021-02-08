using Microsoft.EntityFrameworkCore;

namespace Intereceptors.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}