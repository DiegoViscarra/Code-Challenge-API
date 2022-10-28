using Microsoft.EntityFrameworkCore;
using SchedulingAPI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Data.Repositories.StudentRepository
{
    public class StudentRepository : IStudentRepository
    {
        private SchedulingDbContext dbContext;
        public StudentRepository(SchedulingDbContext dbContext)
        {
            this.dbContext = dbContext;
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
