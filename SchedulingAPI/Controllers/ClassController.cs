using Microsoft.AspNetCore.Mvc;
using SchedulingAPI.Models.DTOs;
using SchedulingAPI.Services.ClassService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService classService;
        public ClassController(IClassService classService)
        {
            this.classService = classService;
        }

        [HttpGet]
        public async Task<ActionResult<SimpleClassDTO>> GetAllClasses()
        {
            try
            {
                var classes = await classService.GetAllClasses();
                return Ok(classes);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<SimpleClassDTO>> GetClassByCode(int code)
        {
            try
            {
                var course = await classService.GetClassByCode(code);
                return Ok(course);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }

        [HttpPost]
        public async Task<ActionResult<SimpleClassDTO>> PostClass([FromBody] SimpleClassDTO simpleClassDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdClass = await classService.AddClass(simpleClassDTO);
                return Created($"/api/class/{createdClass.Code}", createdClass);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }

        [HttpPut("{code}")]
        public async Task<ActionResult<SimpleClassDTO>> PutClass(int code, [FromBody] SimpleClassDTO simpleClassDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var classUpdated = await classService.UpdateClass(code, simpleClassDTO);
                return Ok(classUpdated);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }
    }
}
