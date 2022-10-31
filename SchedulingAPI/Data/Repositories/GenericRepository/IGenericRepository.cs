using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> condition);
        Task<T> GetOneByConditionAsync(Expression<Func<T, bool>> condition);
        Task<List<T>> AddAllAsync(List<T> entity);
        Task<T> AddOneAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAllAsync(List<T> entity);
        Task<bool> DeleteOneAsync(T entity);
    }

}
