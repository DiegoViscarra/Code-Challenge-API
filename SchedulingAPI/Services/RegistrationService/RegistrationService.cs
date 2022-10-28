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

namespace SchedulingAPI.Services.RegistrationService
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IRegistrationRepository repository;
        private readonly IClassRepository classRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        public RegistrationService(IRegistrationRepository repository, IClassRepository classRepository, IStudentRepository studentRepository, IMapper mapper)
        {
            this.repository = repository;
            this.classRepository = classRepository;
            this.studentRepository = studentRepository;
            this.mapper = mapper;
        }

        public async Task<RegistrationToStudentDTO> RegisterClasses(RegistrationToStudentDTO registrationToStudentDTO)
        {
            if (await ValidateRegistrationToStudent(registrationToStudentDTO))
            {
                List<RegistrationDTO> registrationsDTOs = new List<RegistrationDTO>();
                foreach (var classDTO in registrationToStudentDTO.SimpleClassesDTOs)
                {
                    RegistrationDTO registrationDTO = new RegistrationDTO
                    {
                        Code = classDTO.Code,
                        SimpleClassDTO = classDTO,
                        StudentId = registrationToStudentDTO.SimpleStudentDTO.StudentId,
                        SimpleStudentDTO = registrationToStudentDTO.SimpleStudentDTO
                    };
                    registrationsDTOs.Add(registrationDTO);
                }
                var registrations = mapper.Map<List<Registration>>(registrationsDTOs);
                repository.AddRegistration(registrations);
                if (await repository.SaveChangesAsync())
                    return registrationToStudentDTO;
                throw new Exception("Classes were not registered");
            }
            throw new Exception("There was an error with the DB");
        }

        private async Task<bool> ValidateRegistrationToStudent(RegistrationToStudentDTO registrationToStudentDTO)
        {
            var student = await studentRepository.GetStudent(registrationToStudentDTO.SimpleStudentDTO.StudentId);
            if (student == null)
                throw new Exception("Student not found");
            foreach (var classDTO in registrationToStudentDTO.SimpleClassesDTOs)
            {
                var course = await classRepository.GetClass(classDTO.Code);
                if (course == null)
                    throw new Exception("Class not found");
            }
            return true;
        }

        public async Task<RegistrationToClassDTO> RegisterStudents(RegistrationToClassDTO registrationToClassDTO)
        {
            if (await ValidateRegistrationToClass(registrationToClassDTO))
            {
                List<RegistrationDTO> registrationsDTOs = new List<RegistrationDTO>();
                foreach (var studentDTO in registrationToClassDTO.SimpleStudentsDTOs)
                {
                    RegistrationDTO registrationDTO = new RegistrationDTO
                    {
                        Code = registrationToClassDTO.SimpleClassDTO.Code,
                        SimpleClassDTO = registrationToClassDTO.SimpleClassDTO,
                        StudentId = studentDTO.StudentId,
                        SimpleStudentDTO = studentDTO
                    };
                    registrationsDTOs.Add(registrationDTO);
                }
                var registrations = mapper.Map<List<Registration>>(registrationsDTOs);
                repository.AddRegistration(registrations);
                if (await repository.SaveChangesAsync())
                    return registrationToClassDTO;
                throw new Exception("Students were not registered");
            }
            throw new Exception("There was an error with the DB");
        }

        private async Task<bool> ValidateRegistrationToClass(RegistrationToClassDTO registrationToClassDTO)
        {
            var course = await classRepository.GetClass(registrationToClassDTO.SimpleClassDTO.Code);
            if (course == null)
                throw new Exception("Class not found");
            foreach (var studentDTO in registrationToClassDTO.SimpleStudentsDTOs)
            {
                var student = await studentRepository.GetStudent(studentDTO.StudentId);
                if (student == null)
                    throw new Exception("Student not found");
            }
            return true;
        }
    }
}
