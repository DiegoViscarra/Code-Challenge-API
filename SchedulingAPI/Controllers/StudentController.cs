using Microsoft.AspNetCore.Mvc;
using SchedulingAPI.Models.DTOs;
using SchedulingAPI.Services.StudentService;
using System;
using System.Threading.Tasks;

namespace SchedulingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService studentService;
        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<SimpleStudentDTO>> GetAllStudents()
        {
            try
            {
                var students = await studentService.GetAllStudents();
                return Ok(students);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<SimpleStudentDTO>> GetStudentById(Guid studentId)
        {
            try
            {
                var student = await studentService.GetStudentById(studentId);
                return Ok(student);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }

        [HttpGet("{studentId}/classes")]
        public async Task<ActionResult<SimpleStudentDTO>> GetStudentByIdWithClasses(Guid studentId)
        {
            try
            {
                var student = await studentService.GetStudentByIdWithClasses(studentId);
                return Ok(student);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }

        [HttpPost]
        public async Task<ActionResult<SimpleStudentDTO>> PostStudent([FromBody] SimpleStudentDTO simpleStudentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createdStudent = await studentService.AddStudent(simpleStudentDTO);
                return Created($"/api/student/{createdStudent.StudentId}", createdStudent);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }

        [HttpPut("{studentId}")]
        public async Task<ActionResult<SimpleStudentDTO>> PutStudent(Guid studentId, [FromBody] SimpleStudentDTO simpleStudentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var studentUpdated = await studentService.UpdateStudent(studentId, simpleStudentDTO);
                return Ok(studentUpdated);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }

        [HttpDelete("{studentId}")]
        public async Task<ActionResult<bool>> DeleteStudent(Guid studentId)
        {
            try
            {
                return Ok(await studentService.DeleteStudent(studentId));
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }
    }
}
