using SchedulingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.StudentRepository
{
    public interface IStudentRepository
    {
        void AddStudent(Student student);
        Task<bool> SaveChangesAsync();
    }
}
