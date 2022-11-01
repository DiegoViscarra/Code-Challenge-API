using Microsoft.AspNetCore.Mvc;
using SchedulingAPI.Models.DTOs;
using SchedulingAPI.Services.ClassService;
using System;
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
            var classes = await classService.GetAllClasses();
            return Ok(classes);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<SimpleClassDTO>> GetClassByCode(Guid code)
        {
            var course = await classService.GetClassByCode(code);
            return Ok(course);
        }

        [HttpGet("{code}/students")]
        public async Task<ActionResult<SimpleStudentDTO>> GetClassByCodeWithStudents(Guid code)
        {
            var course = await classService.GetClassByCodeWithStudents(code);
            return Ok(course);
        }

        [HttpPost]
        public async Task<ActionResult<SimpleClassDTO>> PostClass([FromBody] SimpleClassDTO simpleClassDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdClass = await classService.AddClass(simpleClassDTO);
            return Created($"/api/class/{createdClass.Code}", createdClass);
        }

        [HttpPut("{code}")]
        public async Task<ActionResult<SimpleClassDTO>> PutClass(Guid code, [FromBody] SimpleClassDTO simpleClassDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var classUpdated = await classService.UpdateClass(code, simpleClassDTO);
            return Ok(classUpdated);
        }

        [HttpDelete("{code}")]
        public async Task<ActionResult<bool>> DeleteClass(Guid code)
        {
            return Ok(await classService.DeleteClass(code));
        }
    }
}
