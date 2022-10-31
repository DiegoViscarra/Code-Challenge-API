using SchedulingAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchedulingAPI.Services.ClassService
{
    public interface IClassService
    {
        Task<IEnumerable<SimpleClassDTO>> GetAllClasses();
        Task<SimpleClassDTO> GetClassByCode(Guid code);
        Task<ClassDTO> GetClassByCodeWithStudents(Guid code);
        Task<SimpleClassDTO> AddClass(SimpleClassDTO simpleClassDTO);
        Task<SimpleClassDTO> UpdateClass(Guid code, SimpleClassDTO simpleClassDTO);
        Task<bool> DeleteClass(Guid code);
    }
}
