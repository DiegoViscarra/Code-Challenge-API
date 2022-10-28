using Microsoft.EntityFrameworkCore;
using SchedulingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.RegistrationRepository
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private SchedulingDbContext dbContext;
        public RegistrationRepository(SchedulingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Registration> GetRegistration(int code, int studentId)
        {
            IQueryable<Registration> query = dbContext.Registrations;
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(r => r.Code == code && r.StudentId == studentId);
        }

        public void AddRegistration(List<Registration> registrations)
        {
            dbContext.Registrations.AddRange(registrations);
        }

        public async Task DeleteRegistration(int code, int studentId)
        {
            var registration = await dbContext.Registrations.FirstAsync(r => r.Code == code && r.StudentId == studentId);
            dbContext.Registrations.Remove(registration);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await dbContext.SaveChangesAsync()) > 0;
        }
    }
}
