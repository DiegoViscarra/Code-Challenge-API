using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchedulingAPI.Data.Entities;
using SchedulingAPI.Data.UnitOfWork;
using SchedulingAPI.Models.DTOs;
using SchedulingAPI.Services.StudentService;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPITest.UnitTests
{
    [TestClass]
    public class StudentServiceTest : DatabaseTest
    {
        private StudentService studentService;

        [TestInitialize()]
        public async Task Initialize()
        {
            var nameDB = Guid.NewGuid().ToString(); ;
            var mapper = ConfigureAutoMapper();
            var context = ConstructContext(nameDB);

            context.Students.Add(new Student()
            {
                StudentId = new Guid("1a8ad51d-05d1-4fdd-9c21-f5931bcfbdb3"),
                FirstName = "Adrian",
                LastName = "Ayala"
            });
            context.Students.Add(new Student()
            {
                StudentId = new Guid("88594cb9-cc45-48e0-b730-1dbfb91aa3d3"),
                FirstName = "Geraldine",
                LastName = "Galindo"
            });
            context.Students.Add(new Student()
            {
                StudentId = new Guid("30ba2c32-7ba4-414a-b5d1-69674f647918"),
                FirstName = "Suely",
                LastName = "Basto"
            });
            context.Classes.Add(new Class()
            {
                Code = new Guid("6cf51cd3-881a-4974-9d82-8a1a24394363"),
                Title = "Math",
                Description = "Class of math"
            });
            context.Classes.Add(new Class()
            {
                Code = new Guid("87f83d5d-9919-4a18-86b5-4f6726e08990"),
                Title = "Science",
                Description = "Class of science"
            });
            context.Registrations.Add(new Registration()
            {
                StudentId = new Guid("1a8ad51d-05d1-4fdd-9c21-f5931bcfbdb3"),
                Code = new Guid("6cf51cd3-881a-4974-9d82-8a1a24394363")
            });
            context.Registrations.Add(new Registration()
            {
                StudentId = new Guid("1a8ad51d-05d1-4fdd-9c21-f5931bcfbdb3"),
                Code = new Guid("87f83d5d-9919-4a18-86b5-4f6726e08990")
            });
            await context.SaveChangesAsync();

            var context2 = ConstructContext(nameDB);

            var uow = new UnitOfWork(context2);

            studentService = new StudentService(uow, mapper);
        }

        [TestMethod]
        public async Task GetAllStudents()
        {
            var students = await studentService.GetAllStudents();
            Assert.AreEqual("Adrian", students.First().FirstName);
            Assert.AreEqual("Geraldine", students.ElementAt(1).FirstName);
            Assert.AreEqual("Suely", students.Last().FirstName);
            Assert.AreEqual(3, students.ToList().Count);
        }

        [TestMethod]
        public async Task GetStudentByStudentId()
        {
            var student = await studentService.GetStudentById(new Guid("88594cb9-cc45-48e0-b730-1dbfb91aa3d3"));
            Assert.AreEqual("Geraldine", student.FirstName);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetNoExistingStudentByStudentId()
        {
            await studentService.GetStudentById(new Guid("98594cb9-cc45-48e0-b730-1dbfb91aa3d3"));
        }

        [TestMethod]
        public async Task GetStudentByIdWithClasses()
        {
            var student = await studentService.GetStudentByIdWithClasses(new Guid("1a8ad51d-05d1-4fdd-9c21-f5931bcfbdb3"));
            Assert.AreEqual("Adrian", student.FirstName);
            Assert.AreEqual("Math", student.simpleClassesDTOs.First().Title);
            Assert.AreEqual("Science", student.simpleClassesDTOs.Last().Title);
            Assert.AreEqual(2, student.simpleClassesDTOs.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task GetNoExistingStudentByIdWithClasses()
        {
            await studentService.GetStudentByIdWithClasses(new Guid("98594cb9-cc45-48e0-b730-1dbfb91aa3d3"));
        }

        [TestMethod]
        public async Task AddStudent()
        {
            SimpleStudentDTO simpleStudentDTO = new SimpleStudentDTO()
            {
                StudentId = new Guid("ecdc8450-7372-44b2-9e55-7a565128109e"),
                FirstName = "Maria",
                LastName = "Romero"
            };
            var newStudent = await studentService.AddStudent(simpleStudentDTO);
            Assert.AreEqual("Maria", newStudent.FirstName);
            Assert.AreEqual("Romero", newStudent.LastName);
            var students = await studentService.GetAllStudents();
            Assert.AreEqual(4, students.ToList().Count);
        }

        [TestMethod]
        public async Task UpdateStudent()
        {
            SimpleStudentDTO simpleStudentDTO = new SimpleStudentDTO()
            {
                StudentId = new Guid("88594cb9-cc45-48e0-b730-1dbfb91aa3d3"),
                FirstName = "Maria",
                LastName = "Romero"
            };
            var studentUpdated = await studentService.UpdateStudent(new Guid("88594cb9-cc45-48e0-b730-1dbfb91aa3d3"), simpleStudentDTO);
            Assert.AreEqual("Maria", studentUpdated.FirstName);
            var studentUpdatedDB = await studentService.GetStudentById(new Guid("88594cb9-cc45-48e0-b730-1dbfb91aa3d3"));
            Assert.AreEqual("Romero", studentUpdatedDB.LastName);
            var students = await studentService.GetAllStudents();
            Assert.AreEqual(3, students.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UpdateNoExistingStudent()
        {
            SimpleStudentDTO simpleStudentDTO = new SimpleStudentDTO()
            {
                StudentId = new Guid("88594cb9-cc45-48e0-b730-1dbfb91aa3d3"),
                FirstName = "Maria",
                LastName = "Romero"
            };
            await studentService.UpdateStudent(new Guid("98594cb9-cc45-48e0-b730-1dbfb91aa3d3"), simpleStudentDTO);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task UpdateStudentWithDifferentStudentId()
        {
            SimpleStudentDTO simpleStudentDTO = new SimpleStudentDTO()
            {
                StudentId = new Guid("98594cb9-cc45-48e0-b730-1dbfb91aa3d3"),
                FirstName = "Maria",
                LastName = "Romero"
            };
            await studentService.UpdateStudent(new Guid("88594cb9-cc45-48e0-b730-1dbfb91aa3d3"), simpleStudentDTO);
        }

        [TestMethod]
        public async Task DeleteStudent()
        {
            var isStudentDeleted = await studentService.DeleteStudent(new Guid("30ba2c32-7ba4-414a-b5d1-69674f647918"));
            Assert.AreEqual(true, isStudentDeleted);
            var students = await studentService.GetAllStudents();
            Assert.AreEqual(2, students.ToList().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task DeleteNoExistingStudent()
        {
            await studentService.DeleteStudent(new Guid("98594cb9-cc45-48e0-b730-1dbfb91aa3d3"));
        }
    }
}
