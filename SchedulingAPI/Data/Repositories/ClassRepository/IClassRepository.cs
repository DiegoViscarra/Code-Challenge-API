using SchedulingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.ClassRepository
{
    public interface IClassRepository
    {
        void AddClass(Class course);
        Task<bool> SaveChangesAsync();
    }
}
