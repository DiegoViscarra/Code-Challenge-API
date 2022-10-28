using Microsoft.EntityFrameworkCore;
using SchedulingAPI.Data.Entities;

namespace SchedulingAPI.Data
{
    public class SchedulingDbContext : DbContext
    {
        public SchedulingDbContext(DbContextOptions<SchedulingDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Class>().ToTable("Classes");
            modelBuilder.Entity<Class>().Property(c => c.Code).ValueGeneratedOnAdd();

            modelBuilder.Entity<Student>().ToTable("Students");
            modelBuilder.Entity<Student>().Property(s => s.StudentId).ValueGeneratedOnAdd();

            modelBuilder.Entity<Registration>().ToTable("Registrations");
            modelBuilder.Entity<Registration>().HasKey(r => new { r.Code, r.StudentId });
        }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Registration> Registrations { get; set; }
    }
}
