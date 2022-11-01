using AutoMapper;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.UnitOfWork;
using SchedulingAPI.Exceptions;
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
            try
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
                    throw new DatabaseException("Classes were not registered");
                }
                throw new DatabaseException("There was an error while registering student to classes");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
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
                throw new AppException("The student is already registered to the class");
        }

        public async Task<RegistrationToClassDTO> RegisterStudents(RegistrationToClassDTO registrationToClassDTO)
        {
            try
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
                    throw new DatabaseException("Students were not registered");
                }
                throw new DatabaseException("There was an error while registering class to students");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
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
            try
            {
                await ValidateClass(code);
                await ValidateStudent(studentId);
                var registration = await ValidateRegistration(code, studentId);
                bool registrationDeleted = await uow.RegistrationRepository.DeleteOneAsync(registration);
                if (registrationDeleted)
                    return true;
                throw new DatabaseException("There was an error while deleting the student from the class");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        private async Task ValidateClass(Guid code)
        {
            var course = await uow.ClassRepository.GetOneByConditionAsync(c => c.Code == code);
            if (course == null)
                throw new NotFoundItemException($"Class with code {code} not found");
        }

        private async Task ValidateStudent(Guid studentId)
        {
            var student = await uow.StudentRepository.GetOneByConditionAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new NotFoundItemException($"Student with studentId {studentId} not found");
        }

        private async Task<Registration> ValidateRegistration(Guid code, Guid studentId)
        {
            var registration = await uow.RegistrationRepository.GetOneByConditionAsync(r => r.Code == code && r.StudentId == studentId);
            if (registration == null)
                throw new NotFoundItemException($"Student with studentId {studentId} not registered to class with code {code}");
            return registration;
        }
    }
}
