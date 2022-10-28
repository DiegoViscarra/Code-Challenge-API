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

        public void AddClass(Class course)
        {
            dbContext.Classes.Add(course);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await dbContext.SaveChangesAsync()) > 0;
        }
    }
}
