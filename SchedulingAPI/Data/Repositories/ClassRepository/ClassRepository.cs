using Microsoft.EntityFrameworkCore;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.Repositories.RegistrationRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.ClassRepository
{
    public class ClassRepository : IClassRepository
    {
        private SchedulingDbContext dbContext;
        private readonly IRegistrationRepository registrationRepository;
        public ClassRepository(SchedulingDbContext dbContext, IRegistrationRepository registrationRepository)
        {
            this.dbContext = dbContext;
            this.registrationRepository = registrationRepository;
        }

        public async Task<IEnumerable<Class>> GetAllClasses()
        {
            IQueryable<Class> query = dbContext.Classes;
            query = query.AsNoTracking();
            return await query.ToArrayAsync();
        }

        public async Task<Class> GetClassByCode(int code)
        {
            IQueryable<Class> query = dbContext.Classes;
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(c => c.Code == code);
        }

        public void AddClass(Class course)
        {
            dbContext.Classes.Add(course);
        }

        public void UpdateClass(int code, Class course)
        {
            var classToUpdate = dbContext.Classes.Single(c => c.Code == code);
            classToUpdate.Title = course.Title;
            classToUpdate.Description = course.Description;
        }

        public async Task DeleteClass(int code)
        {
            var classToDelete = await dbContext.Classes.SingleAsync(c => c.Code == code);
            await DeleteRegistrationsFromClass(code);
            dbContext.Classes.Remove(classToDelete);
        }

        private async Task DeleteRegistrationsFromClass(int code)
        {
            var registrationsFromClass = dbContext.Registrations.Where(r => r.Code == code);
            foreach (var registration in registrationsFromClass)
            {
                await registrationRepository.DeleteRegistration(registration.Code, registration.StudentId);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await dbContext.SaveChangesAsync()) > 0;
        }

        public void DetachEntity<T>(T entity) where T : class
        {
            dbContext.Entry(entity).State = EntityState.Detached;
        }
    }
}
