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

        public async Task<SimpleStudentDTO> UpdateStudent(int studentId, SimpleStudentDTO simpleStudentDTO)
        {
            var studentToUpdate = await repository.GetStudent(studentId);
            if (studentToUpdate == null)
                throw new Exception("Student not found");
            if (simpleStudentDTO.StudentId != 0 && simpleStudentDTO.StudentId != studentId)
                throw new Exception("Path Id and Body Id have to be the same");
            if (simpleStudentDTO.FirstName == null)
                simpleStudentDTO.FirstName = studentToUpdate.FirstName;
            if (simpleStudentDTO.LastName == null)
                simpleStudentDTO.LastName = studentToUpdate.LastName;
            simpleStudentDTO.StudentId = studentId;
            repository.DetachEntity(studentToUpdate);
            var student = mapper.Map<Student>(simpleStudentDTO);
            repository.UpdateStudent(studentId, student);
            if (await repository.SaveChangesAsync())
                return mapper.Map<SimpleStudentDTO>(student);
            throw new Exception("There was an error with the DB");
        }
    }
}
