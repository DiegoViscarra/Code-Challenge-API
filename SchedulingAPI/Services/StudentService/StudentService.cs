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
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SimpleStudentDTO>> GetAllStudents()
        {
            var students = await studentRepository.GetAllStudents();
            var simpleStudentDTOs = mapper.Map<IEnumerable<SimpleStudentDTO>>(students);
            return simpleStudentDTOs;
        }

        public async Task<SimpleStudentDTO> GetStudentById(int studentId)
        {
            var student = await studentRepository.GetStudentById(studentId);
            if (student == null)
                throw new Exception("Student not found");
            var simpleStudentDTO = mapper.Map<SimpleStudentDTO>(student);
            return simpleStudentDTO;
        }

        public async Task<SimpleStudentDTO> AddStudent(SimpleStudentDTO simpleStudentDTO)
        {
            var student = mapper.Map<Student>(simpleStudentDTO);
            studentRepository.AddStudent(student);
            if (await studentRepository.SaveChangesAsync())
                return mapper.Map<SimpleStudentDTO>(student);
            throw new Exception("Student was not added");
        }

        public async Task<SimpleStudentDTO> UpdateStudent(int studentId, SimpleStudentDTO simpleStudentDTO)
        {
            var studentToUpdate = await studentRepository.GetStudentById(studentId);
            if (studentToUpdate == null)
                throw new Exception("Student not found");
            if (simpleStudentDTO.StudentId != 0 && simpleStudentDTO.StudentId != studentId)
                throw new Exception("Path Id and Body Id have to be the same");
            if (simpleStudentDTO.FirstName == null)
                simpleStudentDTO.FirstName = studentToUpdate.FirstName;
            if (simpleStudentDTO.LastName == null)
                simpleStudentDTO.LastName = studentToUpdate.LastName;
            studentRepository.DetachEntity(studentToUpdate);
            var student = mapper.Map<Student>(simpleStudentDTO);
            studentRepository.UpdateStudent(studentId, student);
            if (await studentRepository.SaveChangesAsync())
                return mapper.Map<SimpleStudentDTO>(student);
            throw new Exception("There was an error with the DB");
        }
    }
}
