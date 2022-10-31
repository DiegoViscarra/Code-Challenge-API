using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.Repositories.GenericRepository;

namespace SchedulingAPI.Data.Repositories.StudentRepository
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(SchedulingDbContext schedulingDbContext) : base(schedulingDbContext) {}
    }
}
