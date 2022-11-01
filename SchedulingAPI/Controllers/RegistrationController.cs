using Microsoft.AspNetCore.Mvc;
using SchedulingAPI.Models.DTOs;
using SchedulingAPI.Services.RegistrationService;
using System;
using System.Threading.Tasks;

namespace SchedulingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService registrationService;
        public RegistrationController(IRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        [HttpPost("classes")]
        public async Task<ActionResult<RegistrationToClassDTO>> PostRegistrationClasses([FromBody] RegistrationToStudentDTO registrationToStudentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdRegistration = await registrationService.RegisterClasses(registrationToStudentDTO);
            return Created($"/api/registration/classes", createdRegistration);
        }

        [HttpPost("students")]
        public async Task<ActionResult<RegistrationToClassDTO>> PostRegistrationStudents([FromBody] RegistrationToClassDTO registrationToClassDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var createdRegistration = await registrationService.RegisterStudents(registrationToClassDTO);
            return Created($"/api/registration/students", createdRegistration);
        }

        [HttpDelete("{code}/class/{studentId}/student")]
        public async Task<ActionResult<bool>> Delete(Guid code, Guid studentId)
        {
            return Ok(await registrationService.DeleteRegistration(code, studentId));
        }
    }
}
