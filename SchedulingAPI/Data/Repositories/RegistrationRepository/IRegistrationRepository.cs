using SchedulingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.RegistrationRepository
{
    public interface IRegistrationRepository
    {
        void AddRegistration(List<Registration> registrations);
        Task<bool> SaveChangesAsync();
    }
}
