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
        private readonly IClassRepository repository;
        private readonly IMapper mapper;
        public ClassService(IClassRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<SimpleClassDTO> AddClass(SimpleClassDTO simpleClassDTO)
        {
            var course = mapper.Map<Class>(simpleClassDTO);
            repository.AddClass(course);
            if (await repository.SaveChangesAsync())
                return mapper.Map<SimpleClassDTO>(course);
            throw new Exception("Class was not added");
        }

        public async Task<SimpleClassDTO> UpdateClass(int code, SimpleClassDTO simpleClassDTO)
        {
            var classToUpdate = await repository.GetClass(code);
            if (classToUpdate == null)
                throw new Exception("Class not found");
            if (simpleClassDTO.Code != 0 && simpleClassDTO.Code != code)
                throw new Exception("Path Id and Body Id have to be the same");
            if (simpleClassDTO.Title == null)
                simpleClassDTO.Title = classToUpdate.Title;
            if (simpleClassDTO.Description == null)
                simpleClassDTO.Description = classToUpdate.Description;
            repository.DetachEntity(classToUpdate);
            var course = mapper.Map<Class>(simpleClassDTO);
            repository.UpdateClass(code, course);
            if (await repository.SaveChangesAsync())
                return mapper.Map<SimpleClassDTO>(course);
            throw new Exception("There was an error with the DB");
        }
    }
}
