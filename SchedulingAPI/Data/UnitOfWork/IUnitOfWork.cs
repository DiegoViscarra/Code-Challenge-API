using SchedulingAPI.Data.Repositories.ClassRepository;
using SchedulingAPI.Data.Repositories.RegistrationRepository;
using SchedulingAPI.Data.Repositories.StudentRepository;

namespace SchedulingAPI.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IClassRepository ClassRepository { get; }
        IRegistrationRepository RegistrationRepository { get; }
        IStudentRepository StudentRepository { get; }
        void Commit();
        void Rollback();
    }
}
