using SchedulingAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Services.ClassService
{
    public interface IClassService
    {
        Task<IEnumerable<SimpleClassDTO>> GetAllClasses();
        Task<SimpleClassDTO> GetClassByCode(int code);
        Task<ClassDTO> GetClassByCodeWithStudents(int code);
        Task<SimpleClassDTO> AddClass(SimpleClassDTO simpleClassDTO);
        Task<SimpleClassDTO> UpdateClass(int code, SimpleClassDTO simpleClassDTO);
    }
}
