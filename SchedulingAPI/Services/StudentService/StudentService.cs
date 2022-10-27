using AutoMapper;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.Repositories.StudentRepository;
using SchedulingAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository repository;
        private readonly IMapper mapper;
        public StudentService(IStudentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<SimpleStudentDTO> AddStudent(SimpleStudentDTO simpleStudentDTO)
        {
            var student = mapper.Map<Student>(simpleStudentDTO);
            repository.AddStudent(student);
            if (await repository.SaveChangesAsync())
                return mapper.Map<SimpleStudentDTO>(student);
            throw new Exception("Student was not added");
        }
    }
}
