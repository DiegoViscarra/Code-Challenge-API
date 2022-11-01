using AutoMapper;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.UnitOfWork;
using SchedulingAPI.Exceptions;
using SchedulingAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchedulingAPI.Services.ClassService
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;
        public ClassService(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<SimpleClassDTO>> GetAllClasses()
        {
            try
            {
                var classes = await uow.ClassRepository.GetAllAsync();
                var SimpleClassesDTOs = mapper.Map<IEnumerable<SimpleClassDTO>>(classes);
                return SimpleClassesDTOs;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task<SimpleClassDTO> GetClassByCode(Guid code)
        {
            try
            {
                var course = await ValidateClass(code);
                var simpleClassDTO = mapper.Map<SimpleClassDTO>(course);
                return simpleClassDTO;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task<ClassDTO> GetClassByCodeWithStudents(Guid code)
        {
            try
            {
                var course = await ValidateClass(code);
                var classDTO = mapper.Map<ClassDTO>(course);
                var registrations = await uow.RegistrationRepository.GetAllByConditionAsync(r => r.Code == code);
                if (registrations == null)
                    throw new NotFoundItemException($"Class {course.Title} registrations not found");
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
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task<SimpleClassDTO> AddClass(SimpleClassDTO simpleClassDTO)
        {
            try
            {
                simpleClassDTO.Code = new Guid();
                var course = mapper.Map<Class>(simpleClassDTO);
                var createdClass = await uow.ClassRepository.AddOneAsync(course);
                if (createdClass != null)
                    return mapper.Map<SimpleClassDTO>(course);
                throw new DatabaseException($"Class {course.Title} was not added");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task<SimpleClassDTO> UpdateClass(Guid code, SimpleClassDTO simpleClassDTO)
        {
            try
            {
                var classToUpdate = await ValidateClass(code);
                if (simpleClassDTO.Code != null && simpleClassDTO.Code != code)
                    throw new AppException("Path Code and Body Code have to be the same");
                classToUpdate.Title = simpleClassDTO.Title;
                classToUpdate.Description = simpleClassDTO.Description;
                var classUpdated = await uow.ClassRepository.UpdateAsync(classToUpdate);
                if (classUpdated != null)
                    return mapper.Map<SimpleClassDTO>(classUpdated);
                throw new DatabaseException("There was an error while updating the class");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        public async Task<bool> DeleteClass(Guid code)
        {
            try
            {
                var course = await ValidateClass(code);
                IEnumerable<Registration> registrationsOfClass = await uow.RegistrationRepository.GetAllByConditionAsync(r => r.Code == code);
                List<Registration> ListRegistrationsOfClass = new List<Registration>(registrationsOfClass);
                bool registrationsOfClassDeleted = await uow.RegistrationRepository.DeleteAllAsync(ListRegistrationsOfClass);
                if (registrationsOfClassDeleted)
                {
                    bool courseDeleted = await uow.ClassRepository.DeleteOneAsync(course);
                    if (courseDeleted)
                        return courseDeleted;
                    throw new DatabaseException($"There was an error while deleting the class {course.Title}");
                }
                else
                {
                    throw new DatabaseException($"There was an error while deleting the registrations of the class {course.Title}");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
        }

        private async Task<Class> ValidateClass(Guid code)
        {
            var course = await uow.ClassRepository.GetOneByConditionAsync(c => c.Code == code);
            if (course == null)
                throw new NotFoundItemException($"Class with code {code} not found");
            return course;
        }

        private async Task<Student> ValidateStudent(Guid studentId)
        {
            var student = await uow.StudentRepository.GetOneByConditionAsync(s => s.StudentId == studentId);
            if (student == null)
                throw new NotFoundItemException($"Student with studentId {studentId} not found");
            return student;
        }
    }
}
