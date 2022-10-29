using Microsoft.EntityFrameworkCore;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.Repositories.RegistrationRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.StudentRepository
{
    public class StudentRepository : IStudentRepository
    {
        private SchedulingDbContext dbContext;
        private readonly IRegistrationRepository registrationRepository;
        public StudentRepository(SchedulingDbContext dbContext, IRegistrationRepository registrationRepository)
        {
            this.dbContext = dbContext;
            this.registrationRepository = registrationRepository;
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            IQueryable<Student> query = dbContext.Students;
            query = query.AsNoTracking();
            return await query.ToArrayAsync();
        }

        public async Task<Student> GetStudentById(int studentId)
        {
            IQueryable<Student> query = dbContext.Students;
            query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(s => s.StudentId == studentId);
        }

        public void AddStudent(Student student)
        {
            dbContext.Students.Add(student);
        }

        public void UpdateStudent(int studentId, Student student)
        {
            var studentToUpdate = dbContext.Students.Single(s => s.StudentId == studentId);
            studentToUpdate.FirstName = student.FirstName;
            studentToUpdate.LastName = student.LastName;
        }

        public async Task DeleteStudent(int studentId)
        {
            var studentToDelete = await dbContext.Students.SingleAsync(s => s.StudentId == studentId);
            await DeleteRegistrationsFromStudent(studentId);
            dbContext.Students.Remove(studentToDelete);
        }

        private async Task DeleteRegistrationsFromStudent(int studentId)
        {
            var registrationsFromStudent = dbContext.Registrations.Where(r => r.StudentId == studentId);
            foreach (var registration in registrationsFromStudent)
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
