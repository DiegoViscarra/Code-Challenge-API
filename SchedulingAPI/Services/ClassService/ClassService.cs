using AutoMapper;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.Repositories.ClassRepository;
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
        private readonly IMapper mapper;
        public ClassService(IClassRepository classRepository, IMapper mapper)
        {
            this.classRepository = classRepository;
            this.mapper = mapper;
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
    }
}
