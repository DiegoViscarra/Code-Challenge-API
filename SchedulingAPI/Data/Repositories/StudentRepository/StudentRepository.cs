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

        public void AddStudent(Student student)
        {
            dbContext.Students.Add(student);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await dbContext.SaveChangesAsync()) > 0;
        }
    }
}
