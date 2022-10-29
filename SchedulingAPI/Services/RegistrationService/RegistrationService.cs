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
        private readonly IRegistrationRepository registrationRepository;
        private readonly IClassRepository classRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IMapper mapper;
        public RegistrationService(IRegistrationRepository registrationRepository, IClassRepository classRepository, IStudentRepository studentRepository, IMapper mapper)
        {
            this.registrationRepository = registrationRepository;
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
                    await ValidateNotDoubleRegistration(classDTO.Code, registrationToStudentDTO.SimpleStudentDTO.StudentId);
                    RegistrationDTO registrationDTO = new RegistrationDTO
                    {
                        Code = classDTO.Code,
                        StudentId = registrationToStudentDTO.SimpleStudentDTO.StudentId
                    };
                    registrationsDTOs.Add(registrationDTO);
                }
                var registrations = mapper.Map<List<Registration>>(registrationsDTOs);
                registrationRepository.AddRegistration(registrations);
                if (await registrationRepository.SaveChangesAsync())
                    return registrationToStudentDTO;
                throw new Exception("Classes were not registered");
            }
            throw new Exception("There was an error with the DB");
        }

        private async Task<bool> ValidateRegistrationToStudent(RegistrationToStudentDTO registrationToStudentDTO)
        {
            await ValidateStudent(registrationToStudentDTO.SimpleStudentDTO.StudentId);
            foreach (var classDTO in registrationToStudentDTO.SimpleClassesDTOs)
            {
                await ValidateClass(classDTO.Code);
            }
            return true;
        }

        private async Task ValidateNotDoubleRegistration(int code, int studentId)
        {
            var course = await registrationRepository.GetRegistrationByIds(code, studentId);
            if (course != null)
                throw new Exception("The student is already registered to the class");
        }

        public async Task<RegistrationToClassDTO> RegisterStudents(RegistrationToClassDTO registrationToClassDTO)
        {
            if (await ValidateRegistrationToClass(registrationToClassDTO))
            {
                List<RegistrationDTO> registrationsDTOs = new List<RegistrationDTO>();
                foreach (var studentDTO in registrationToClassDTO.SimpleStudentsDTOs)
                {
                    await ValidateNotDoubleRegistration(registrationToClassDTO.SimpleClassDTO.Code, studentDTO.StudentId);
                    RegistrationDTO registrationDTO = new RegistrationDTO
                    {
                        Code = registrationToClassDTO.SimpleClassDTO.Code,
                        StudentId = studentDTO.StudentId
                    };
                    registrationsDTOs.Add(registrationDTO);
                }
                var registrations = mapper.Map<List<Registration>>(registrationsDTOs);
                registrationRepository.AddRegistration(registrations);
                if (await registrationRepository.SaveChangesAsync())
                    return registrationToClassDTO;
                throw new Exception("Students were not registered");
            }
            throw new Exception("There was an error with the DB");
        }

        private async Task<bool> ValidateRegistrationToClass(RegistrationToClassDTO registrationToClassDTO)
        {
            await ValidateClass(registrationToClassDTO.SimpleClassDTO.Code);
            foreach (var studentDTO in registrationToClassDTO.SimpleStudentsDTOs)
            {
                await ValidateStudent(studentDTO.StudentId);
            }
            return true;
        }

        public async Task<bool> DeleteRegistration(int code, int studentId)
        {
            await ValidateClass(code);
            await ValidateStudent(studentId);
            await ValidateRegistration(code, studentId);
            await registrationRepository.DeleteRegistration(code, studentId);
            if (await registrationRepository.SaveChangesAsync())
                return true;
            return false;
        }

        private async Task ValidateClass(int code)
        {
            var course = await classRepository.GetClassByCode(code);
            if (course == null)
                throw new Exception("Class not found");
        }

        private async Task ValidateStudent(int studentId)
        {
            var student = await studentRepository.GetStudentById(studentId);
            if (student == null)
                throw new Exception("Student not found");
        }

        private async Task ValidateRegistration(int code, int studentId)
        {
            var registration = await registrationRepository.GetRegistrationByIds(code, studentId);
            if (registration == null)
                throw new Exception("Registration not found");
        }
    }
}
