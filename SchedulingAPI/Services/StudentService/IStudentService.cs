using SchedulingAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Services.StudentService
{
    public interface IStudentService
    {
        Task<IEnumerable<SimpleStudentDTO>> GetAllStudents();
        Task<SimpleStudentDTO> GetStudentById(Guid studentId);
        Task<StudentDTO> GetStudentByIdWithClasses(Guid studentId);
        Task<SimpleStudentDTO> AddStudent(SimpleStudentDTO simpleStudentDTO);
        Task<SimpleStudentDTO> UpdateStudent(Guid studentId, SimpleStudentDTO simpleStudentDTO);
        Task<bool> DeleteStudent(Guid studentId);
    }
}
