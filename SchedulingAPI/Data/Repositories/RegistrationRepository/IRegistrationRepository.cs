using SchedulingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.RegistrationRepository
{
    public interface IRegistrationRepository
    {
        Task<Registration> GetRegistrationByIds(int code, int studentId);
        void AddRegistration(List<Registration> registrations);
        Task DeleteRegistration(int code, int studentId);
        Task<bool> SaveChangesAsync();
    }
}
