using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Order.Repository
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> GetById(object id);
        Task<IEnumerable<T>> GetAll();
        Task Insert(T entity);
        Task Delete(T entity);
        Task Update(T entity);
    }
}
