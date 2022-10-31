using AutoMapper;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.UnitOfWork;
using SchedulingAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchedulingAPI.Services.RegistrationService
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public RegistrationService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
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
                var createdRegistrations = await uow.RegistrationRepository.AddAllAsync(registrations);
                if (createdRegistrations != null)
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

        private async Task ValidateNotDoubleRegistration(Guid code, Guid studentId)
        {
            var course = await uow.RegistrationRepository.GetOneByConditionAsync(r => r.Code == code && r.StudentId == studentId);
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
                var createdRegistrations = await uow.RegistrationRepository.AddAllAsync(registrations);
                if (createdRegistrations != null)
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

        public async Task<bool> DeleteRegistration(Guid code, Guid studentId)
        {
            await ValidateClass(code);
            await ValidateStudent(studentId);
            var registration = await ValidateRegistration(code, studentId);
            bool registrationDeleted = await uow.RegistrationRepository.DeleteOneAsync(registration);
            if (registrationDeleted)
                return true;
            throw new Exception("There was an error deleting the student from the class");
        }

        private async Task ValidateClass(Guid code)
        {
            var course = await uow.ClassRepository.GetOneByConditionAsync(c => c.Code == code);
            if (course == null)
                throw new Exception("Class not found");
        }

        private async Task ValidateStudent(Guid studentId)
        {
            var student = await uow.StudentRepository.GetOneByConditionAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new Exception("Student not found");
        }

        private async Task<Registration> ValidateRegistration(Guid code, Guid studentId)
        {
            var registration = await uow.RegistrationRepository.GetOneByConditionAsync(r => r.Code == code && r.StudentId == studentId);
            if (registration == null)
                throw new Exception("Registration not found");
            return registration;
        }
    }
}
