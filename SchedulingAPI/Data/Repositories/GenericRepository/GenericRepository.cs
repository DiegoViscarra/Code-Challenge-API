using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public SchedulingDbContext schedulingDbContext { get; set; }
        public GenericRepository(SchedulingDbContext schedulingDbContext)
        {
            this.schedulingDbContext = schedulingDbContext;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await schedulingDbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> condition)
        {
            return await schedulingDbContext.Set<T>().Where(condition).ToListAsync();
        }

        public async Task<T> GetOneByConditionAsync(Expression<Func<T, bool>> condition)
        {
            return await schedulingDbContext.Set<T>().Where(condition).FirstOrDefaultAsync();
        }

        public async Task<List<T>> AddAllAsync(List<T> entity)
        {
            await schedulingDbContext.Set<T>().AddRangeAsync(entity);
            await schedulingDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> AddOneAsync(T entity)
        {
            await schedulingDbContext.Set<T>().AddAsync(entity);
            await schedulingDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            schedulingDbContext.Entry(entity).State = EntityState.Modified;
            await schedulingDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAllAsync(List<T> entity)
        {
            schedulingDbContext.Set<T>().RemoveRange(entity);
            await schedulingDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOneAsync(T entity)
        {
            schedulingDbContext.Set<T>().Remove(entity);
            await schedulingDbContext.SaveChangesAsync();
            return true;
        }
    }

}
