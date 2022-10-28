using SchedulingAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Services.ClassService
{
    public interface IClassService
    {
        Task<SimpleClassDTO> AddClass(SimpleClassDTO simpleClassDTO);
        Task<SimpleClassDTO> UpdateClass(int code, SimpleClassDTO simpleClassDTO);
    }
}
