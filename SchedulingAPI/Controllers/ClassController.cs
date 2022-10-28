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
        private readonly IClassService service;
        public ClassController(IClassService service)
        {
            this.service = service;
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
                var createdClass = await service.AddClass(simpleClassDTO);
                return Created($"/api/class/{createdClass.Code}", createdClass);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }
    }
}
