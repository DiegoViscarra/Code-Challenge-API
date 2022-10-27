using Microsoft.AspNetCore.Mvc;
using SchedulingAPI.Models.DTOs;
using SchedulingAPI.Services.StudentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService service;
        public StudentController(IStudentService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<ActionResult<SimpleStudentDTO>> Post([FromBody] SimpleStudentDTO simpleStudentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdStudent = await service.AddStudent(simpleStudentDTO);
                return Created($"/api/student/{createdStudent.StudentId}", createdStudent);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }
    }
}
