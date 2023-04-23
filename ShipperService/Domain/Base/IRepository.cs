using System.Threading.Tasks;

namespace Base.Domain
{
    public interface IRepository<T,Tid> where T : IAggregateRoot
    {
        T Add(T entity);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        Task<T> GetAsync(Tid id);
    }
}
