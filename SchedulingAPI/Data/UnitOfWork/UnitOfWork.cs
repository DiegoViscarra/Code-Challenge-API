using SchedulingAPI.Data.Repositories.ClassRepository;
using SchedulingAPI.Data.Repositories.RegistrationRepository;
using SchedulingAPI.Data.Repositories.StudentRepository;

namespace SchedulingAPI.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchedulingDbContext schedulingDbContext;
        private readonly IClassRepository classRepository;
        private readonly IRegistrationRepository registrationRepository;
        private readonly IStudentRepository studentRepository;

        public UnitOfWork(SchedulingDbContext schedulingDbContext)
        {
            this.schedulingDbContext = schedulingDbContext;
            classRepository = new ClassRepository(schedulingDbContext);
            registrationRepository = new RegistrationRepository(schedulingDbContext);
            studentRepository = new StudentRepository(schedulingDbContext);
        }

        public IClassRepository ClassRepository
        {
            get { return classRepository; }
        }

        public IRegistrationRepository RegistrationRepository
        {
            get { return registrationRepository; }
        }

        public IStudentRepository StudentRepository
        {
            get { return studentRepository; }
        }

        public void Commit()
        {
            schedulingDbContext.SaveChanges(); 
        }

        public void Rollback()
        {
            schedulingDbContext.Dispose(); 
        }
    }
}
