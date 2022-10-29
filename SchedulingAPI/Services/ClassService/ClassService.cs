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

namespace SchedulingAPI.Services.ClassService
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository classRepository;
        private readonly IRegistrationRepository registrationRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        public ClassService(IClassRepository classRepository, IRegistrationRepository registrationRepository, IStudentRepository studentRepository, IMapper mapper)
        {
            this.classRepository = classRepository;
            this.registrationRepository = registrationRepository;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SimpleClassDTO>> GetAllClasses()
        {
            var classes = await classRepository.GetAllClasses();
            var SimpleClassesDTOs = mapper.Map<IEnumerable<SimpleClassDTO>>(classes);
            return SimpleClassesDTOs;
        }

        public async Task<ClassDTO> GetClassByCodeWithStudents(int code)
        {
            var course = await ValidateClass(code);
            var classDTO = mapper.Map<ClassDTO>(course);
            var registrations = await registrationRepository.GetRegistrationsByClassCode(code);
            if (registrations == null)
                throw new Exception("Class registrations not found");
            IEnumerable<RegistrationDTO> registrationsDTOs = mapper.Map<IEnumerable<RegistrationDTO>>(registrations);
            List<SimpleStudentDTO> simpleStudentsDTOs = new List<SimpleStudentDTO>();
            foreach (var registrationDTO in registrationsDTOs)
            {
                var student = await ValidateStudent(registrationDTO.StudentId);
                var simpleStudentDTO = mapper.Map<SimpleStudentDTO>(student);
                simpleStudentsDTOs.Add(simpleStudentDTO);
            }
            classDTO.simpleStudentsDTOs = simpleStudentsDTOs;
            return classDTO;
        }

        public async Task<SimpleClassDTO> GetClassByCode(int code)
        {
            var course = await ValidateClass(code);
            var simpleClassDTO = mapper.Map<SimpleClassDTO>(course);
            return simpleClassDTO;
        }

        public async Task<SimpleClassDTO> AddClass(SimpleClassDTO simpleClassDTO)
        {
            var course = mapper.Map<Class>(simpleClassDTO);
            classRepository.AddClass(course);
            if (await classRepository.SaveChangesAsync())
                return mapper.Map<SimpleClassDTO>(course);
            throw new Exception("Class was not added");
        }

        public async Task<SimpleClassDTO> UpdateClass(int code, SimpleClassDTO simpleClassDTO)
        {
            var classToUpdate = await classRepository.GetClassByCode(code);
            if (classToUpdate == null)
                throw new Exception("Class not found");
            if (simpleClassDTO.Code != 0 && simpleClassDTO.Code != code)
                throw new Exception("Path Id and Body Id have to be the same");
            if (simpleClassDTO.Title == null)
                simpleClassDTO.Title = classToUpdate.Title;
            if (simpleClassDTO.Description == null)
                simpleClassDTO.Description = classToUpdate.Description;
            classRepository.DetachEntity(classToUpdate);
            var course = mapper.Map<Class>(simpleClassDTO);
            classRepository.UpdateClass(code, course);
            if (await classRepository.SaveChangesAsync())
                return mapper.Map<SimpleClassDTO>(course);
            throw new Exception("There was an error with the DB");
        }

        private async Task<Class> ValidateClass(int code)
        {
            var course = await classRepository.GetClassByCode(code);
            if (course == null)
                throw new Exception("Class not found");
            return course;
        }

        private async Task<Student> ValidateStudent(int studentId)
        {
            var student = await studentRepository.GetStudentById(studentId);
            if (student == null)
                throw new Exception("Student not found");
            return student;
        }
    }
}
