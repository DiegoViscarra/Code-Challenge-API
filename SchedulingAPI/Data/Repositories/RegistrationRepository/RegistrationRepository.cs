using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.Repositories.GenericRepository;

namespace SchedulingAPI.Data.Repositories.RegistrationRepository
{
    public class RegistrationRepository : GenericRepository<Registration>, IRegistrationRepository
    {
        public RegistrationRepository(SchedulingDbContext schedulingDbContext) : base(schedulingDbContext) { }
    }
}
