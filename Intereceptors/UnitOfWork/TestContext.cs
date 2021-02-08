using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Intereceptors.UnitOfWork
{
    public class TestContext: Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Entity> Entities { get; set; }

        public TestContext(DbContextOptions options): base(options)
        {
        }
    }

    public class Entity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString() => Name;
    }

    public class EntityRepository
    {
        private readonly TestContext _context;

        public EntityRepository(TestContext context)
        {
            _context = context;
        }

        public void Add(Entity entity)
        {
            _context.Entities.Add(entity);
        }

        public IEnumerable<Entity> GetAll()
        {
            return _context.Entities.ToList();
        }
    }

    public interface IUnitOfWork
    {
        public EntityRepository Entities { get; set; }

        public void SaveChanges();
    }

    public class UnitOfWork: IUnitOfWork, IDisposable
    {
        private readonly TestContext _context;

        public EntityRepository Entities { get; set; }

        public UnitOfWork(TestContext context)
        {
            _context = context;
            Entities = new EntityRepository(context);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}