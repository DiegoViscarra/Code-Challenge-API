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

        public async Task<RegistrationToClassDTO> RegisterStudents(RegistrationToClassDTO registrationToClassDTO)
        {
            if (await ValidateRegistrationToClass(registrationToClassDTO))
            {
                List<RegistrationDTO> registrationsDTOs = new List<RegistrationDTO>();
                foreach (var studentDTO in registrationToClassDTO.StudentsDTOs)
                {
                    RegistrationDTO registrationDTO = new RegistrationDTO
                    {
                        Code = registrationToClassDTO.ClassDTO.Code,
                        ClassDTO = registrationToClassDTO.ClassDTO,
                        StudentId = studentDTO.StudentId,
                        StudentDTO = studentDTO
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
            var course = await classRepository.GetClass(registrationToClassDTO.ClassDTO.Code);
            if (course == null)
                throw new Exception("Class not found");
            foreach (var studentDTO in registrationToClassDTO.StudentsDTOs)
            {
                var student = await studentRepository.GetStudent(studentDTO.StudentId);
                if (student == null)
                    throw new Exception("Student not found");
            }
            return true;
        }
    }
}
