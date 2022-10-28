using Microsoft.AspNetCore.Mvc;
using SchedulingAPI.Models.DTOs;
using SchedulingAPI.Services.RegistrationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService service;
        public RegistrationController(IRegistrationService service)
        {
            this.service = service;
        }

        [HttpPost("/students")]
        public async Task<ActionResult<RegistrationToClassDTO>> PostRegistrationStudents([FromBody] RegistrationToClassDTO registrationToClassDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdRegistration = await service.RegisterStudents(registrationToClassDTO);
                return Created($"/api/registration/students", createdRegistration);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }
    }
}
