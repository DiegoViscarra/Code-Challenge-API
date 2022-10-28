using SchedulingAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingAPI.Services.RegistrationService
{
    public interface IRegistrationService
    {
        Task<RegistrationToStudentDTO> RegisterClasses(RegistrationToStudentDTO registrationToStudentDTO);
        Task<RegistrationToClassDTO> RegisterStudents(RegistrationToClassDTO registrationToClassDTO);
        Task<bool> DeleteRegistration(int code, int studentId);
    }
}
