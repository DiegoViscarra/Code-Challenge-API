using SchedulingAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Services.StudentService
{
    public interface IStudentService
    {
        Task<SimpleStudentDTO> AddStudent(SimpleStudentDTO simpleStudentDTO);
        Task<SimpleStudentDTO> UpdateStudent(int studentId, SimpleStudentDTO student);
    }
}
