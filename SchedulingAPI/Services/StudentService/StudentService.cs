using AutoMapper;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.UnitOfWork;
using SchedulingAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchedulingAPI.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public StudentService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SimpleStudentDTO>> GetAllStudents()
        {
            var students = await uow.StudentRepository.GetAllAsync();
            var simpleStudentDTOs = mapper.Map<IEnumerable<SimpleStudentDTO>>(students);
            return simpleStudentDTOs;
        }

        public async Task<SimpleStudentDTO> GetStudentById(Guid studentId)
        {
            var student = await ValidateStudent(studentId);
            var simpleStudentDTO = mapper.Map<SimpleStudentDTO>(student);
            return simpleStudentDTO;
        }

        public async Task<StudentDTO> GetStudentByIdWithClasses(Guid studentId)
        {
            var student = await ValidateStudent(studentId);
            var studentDTO = mapper.Map<StudentDTO>(student);
            var registrations = await uow.RegistrationRepository.GetAllByConditionAsync(r => r.StudentId == studentId);
            if (registrations == null)
                throw new Exception($"Student {student.FirstName} registrations not found");
            IEnumerable<RegistrationDTO> registrationsDTOs = mapper.Map<IEnumerable<RegistrationDTO>>(registrations);
            List<SimpleClassDTO> simpleClassesDTOs = new List<SimpleClassDTO>();
            foreach (var registrationDTO in registrationsDTOs)
            {
                var course = await ValidateClass(registrationDTO.Code);
                var simpleClassDTO = mapper.Map<SimpleClassDTO>(course);
                simpleClassesDTOs.Add(simpleClassDTO);
            }
            studentDTO.simpleClassesDTOs = simpleClassesDTOs;
            return studentDTO;
        }


        public async Task<SimpleStudentDTO> AddStudent(SimpleStudentDTO simpleStudentDTO)
        {
            simpleStudentDTO.StudentId = new Guid();
            var student = mapper.Map<Student>(simpleStudentDTO);
            var createdStudent = await uow.StudentRepository.AddOneAsync(student);
            if (createdStudent  != null)
                return mapper.Map<SimpleStudentDTO>(student);
            throw new Exception($"Student {student.FirstName} was not added");
        }

        public async Task<SimpleStudentDTO> UpdateStudent(Guid studentId, SimpleStudentDTO simpleStudentDTO)
        {
            var studentToUpdate = await ValidateStudent(studentId);
            if (simpleStudentDTO.StudentId != null && simpleStudentDTO.StudentId != studentId)
                throw new Exception("Path Id and Body Id have to be the same");
            studentToUpdate.FirstName = simpleStudentDTO.FirstName;
            studentToUpdate.LastName = simpleStudentDTO.LastName;
            var studentUpdated = await uow.StudentRepository.UpdateAsync(studentToUpdate);
            if (studentUpdated != null)
                return mapper.Map<SimpleStudentDTO>(studentUpdated);
            throw new Exception("There was an error with the DB");
        }

        public async Task<bool> DeleteStudent(Guid studentId)
        {
            var student = await ValidateStudent(studentId);
            IEnumerable<Registration> registrationsOfStudent = await uow.RegistrationRepository.GetAllByConditionAsync(r => r.StudentId == studentId);
            List<Registration> ListRegistrationsOfStudent = new List<Registration>(registrationsOfStudent);
            bool registrationsOfStudentDeleted = await uow.RegistrationRepository.DeleteAllAsync(ListRegistrationsOfStudent);
            if (registrationsOfStudentDeleted)
            {
                bool studentDeleted = await uow.StudentRepository.DeleteOneAsync(student);
                if (studentDeleted)
                    return studentDeleted;
                throw new Exception($"There was an error deleting the student {student.FirstName}");
            }
            else
            {
                throw new Exception($"There was an error deleting the registrations of the student {student.FirstName}");
            }
        }

        private async Task<Student> ValidateStudent(Guid studentId)
        {
            var student = await uow.StudentRepository.GetOneByConditionAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new Exception("Student not found");
            return student;
        }

        private async Task<Class> ValidateClass(Guid code)
        {
            var course = await uow.ClassRepository.GetOneByConditionAsync(c => c.Code == code);
            if (course == null)
                throw new Exception("Class not found");
            return course;
        }
    }
}
