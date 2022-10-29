using SchedulingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.ClassRepository
{
    public interface IClassRepository
    {
        Task<Class> GetClassByCode(int code);
        void AddClass(Class course);
        void UpdateClass(int code, Class course);
        Task<bool> SaveChangesAsync();
        void DetachEntity<T>(T entity) where T : class;
    }
}
