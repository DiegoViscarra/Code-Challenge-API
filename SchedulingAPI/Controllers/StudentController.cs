﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<SimpleStudentDTO>> GetStudentById(int id)
        {
            try
            {
                var student = await studentService.GetStudentById(id);
                return Ok(student);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }

        [HttpGet("{id}/classes")]
        public async Task<ActionResult<SimpleStudentDTO>> GetStudentByIdWithClasses(int id)
        {
            try
            {
                var student = await studentService.GetStudentByIdWithClasses(id);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<SimpleStudentDTO>> PutStudent(int id, [FromBody] SimpleStudentDTO simpleStudentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var studentUpdated = await studentService.UpdateStudent(id, simpleStudentDTO);
                return Ok(studentUpdated);
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            try
            {
                return Ok(await studentService.DeleteStudent(id));
            }
            catch (Exception e)
            {
                throw new Exception("Error:", e);
            }
        }
    }
}
