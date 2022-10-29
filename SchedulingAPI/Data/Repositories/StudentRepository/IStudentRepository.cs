using SchedulingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.StudentRepository
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<Student> GetStudentById(int studentId);
        void AddStudent(Student student);
        void UpdateStudent(int studentId, Student student);
        Task DeleteStudent(int studentId);
        Task<bool> SaveChangesAsync();
        void DetachEntity<T>(T entity) where T : class;
    }
}
