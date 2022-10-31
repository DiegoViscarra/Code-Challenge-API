using AutoMapper;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Models.DTOs;

namespace SchedulingAPI.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Class, ClassDTO>().ReverseMap();
            CreateMap<Class, SimpleClassDTO>().ReverseMap();
            CreateMap<Registration, RegistrationDTO>().ReverseMap();
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Student, SimpleStudentDTO>().ReverseMap();
        }
    }
}
