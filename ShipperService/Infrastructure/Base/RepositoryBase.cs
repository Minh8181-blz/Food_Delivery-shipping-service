using Base.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Base.Infrastructure
{
    public class RepositoryBase<TContext, TEntity, Tid> : IRepository<TEntity, Tid>
        where TContext : DbContext
        where TEntity : Entity<Tid>, IAggregateRoot
    {
        protected readonly TContext _context;

        public RepositoryBase(TContext context)
        {
            _context = context;
        }

        public virtual TEntity Add(TEntity entity)
        {
            return _context.Set<TEntity>().Add(entity).Entity;
        }
        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            return (await _context.Set<TEntity>().AddAsync(entity)).Entity;
        }

        public virtual async Task<TEntity> GetAsync(Tid id)
        {
            var entity = await _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(o => o.Id.Equals(id));

            return entity;
        }

        public virtual void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
