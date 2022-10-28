using Microsoft.EntityFrameworkCore;
using SchedulingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.ClassRepository
{
    public class ClassRepository : IClassRepository
    {
        private SchedulingDbContext dbContext;
        public ClassRepository(SchedulingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Class> GetClass(int code)
        {
            IQueryable<Class> query = dbContext.Classes;
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(c => c.Code == code);
        }

        public void AddClass(Class course)
        {
            dbContext.Classes.Add(course);
        }

        public void UpdateClass(int code, Class course)
        {
            var classToUpdate = dbContext.Classes.Single(c => c.Code == code);
            classToUpdate.Title = course.Title;
            classToUpdate.Description = course.Description;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await dbContext.SaveChangesAsync()) > 0;
        }

        public void DetachEntity<T>(T entity) where T : class
        {
            dbContext.Entry(entity).State = EntityState.Detached;
        }
    }
}
