using AutoMapper;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Models.DTOs;

namespace SchedulingAPI.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            this.CreateMap<Class, ClassDTO>().ReverseMap();
            this.CreateMap<Registration, RegistrationDTO>().ReverseMap();
            this.CreateMap<Student, StudentDTO>().ReverseMap();
            this.CreateMap<Student, SimpleStudentDTO>().ReverseMap();
        }
    }
}
