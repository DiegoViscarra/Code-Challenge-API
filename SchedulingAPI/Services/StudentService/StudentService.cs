using AutoMapper;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.Repositories.ClassRepository;
using SchedulingAPI.Data.Repositories.RegistrationRepository;
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
        private readonly IClassRepository classRepository;
        private readonly IRegistrationRepository registrationRepository;
        private readonly IMapper mapper;
        public StudentService(IStudentRepository studentRepository, IRegistrationRepository registrationRepository, IClassRepository classRepository, IMapper mapper)
        {
            this.studentRepository = studentRepository;
            this.classRepository = classRepository;
            this.registrationRepository = registrationRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SimpleStudentDTO>> GetAllStudents()
        {
            var students = await studentRepository.GetAllStudents();
            var simpleStudentDTOs = mapper.Map<IEnumerable<SimpleStudentDTO>>(students);
            return simpleStudentDTOs;
        }

        public async Task<StudentDTO> GetStudentByIdWithClasses(int studentId)
        {
            var student = await ValidateStudent(studentId);
            var studentDTO = mapper.Map<StudentDTO>(student);
            var registrations = await registrationRepository.GetRegistrationsByStudentId(studentId);
            if (registrations == null)
                throw new Exception("Student registrations not found");
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

        public async Task<SimpleStudentDTO> GetStudentById(int studentId)
        {
            var student = await ValidateStudent(studentId);
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
            var studentToUpdate = await ValidateStudent(studentId);
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

        public async Task<bool> DeleteStudent(int studentId)
        {
            await ValidateStudent(studentId);
            await studentRepository.DeleteStudent(studentId);
            if (await studentRepository.SaveChangesAsync())
                return true;
            return false;
        }

        private async Task<Student> ValidateStudent(int studentId)
        {
            var student = await studentRepository.GetStudentById(studentId);
            if (student == null)
                throw new Exception("Student not found");
            return student;
        }

        private async Task<Class> ValidateClass(int code)
        {
            var course = await classRepository.GetClass(code);
            if (course == null)
                throw new Exception("Class not found");
            return course;
        }
    }
}
