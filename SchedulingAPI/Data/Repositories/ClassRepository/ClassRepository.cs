using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.Repositories.GenericRepository;

namespace SchedulingAPI.Data.Repositories.ClassRepository
{
    public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
        public ClassRepository(SchedulingDbContext schedulingDbContext) : base (schedulingDbContext) {}
    }
}
