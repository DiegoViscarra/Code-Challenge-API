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
    }
}
