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

        public void AddRegistration(List<Registration> registrations)
        {
            dbContext.Registrations.AddRange(registrations);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await dbContext.SaveChangesAsync()) > 0;
        }
    }
}
