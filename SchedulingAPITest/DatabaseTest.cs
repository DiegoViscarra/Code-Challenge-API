using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchedulingAPI.Data;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Models.DTOs;

namespace SchedulingAPITest
{
    public class DatabaseTest
    {
        protected SchedulingDbContext ConstructContext(string nameDB)
        {
            var options = new DbContextOptionsBuilder<SchedulingDbContext>()
                .UseInMemoryDatabase(nameDB).Options;

            var dbContext = new SchedulingDbContext(options);
            return dbContext;
        }

        protected IMapper ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Class, ClassDTO>().ReverseMap();
                cfg.CreateMap<Class, SimpleClassDTO>().ReverseMap();
                cfg.CreateMap<Registration, RegistrationDTO>().ReverseMap();
                cfg.CreateMap<Student, StudentDTO>().ReverseMap();
                cfg.CreateMap<Student, SimpleStudentDTO>().ReverseMap();
            });

            return config.CreateMapper();
        }
    }
}