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
        Task<SimpleStudentDTO> GetStudentById(int studentId);
        Task<StudentDTO> GetStudentByIdWithClasses(int studentId);
        Task<SimpleStudentDTO> AddStudent(SimpleStudentDTO simpleStudentDTO);
        Task<SimpleStudentDTO> UpdateStudent(int studentId, SimpleStudentDTO simpleStudentDTO);
    }
}
