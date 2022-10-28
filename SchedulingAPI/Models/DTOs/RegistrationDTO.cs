using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Models.DTOs
{
    public class RegistrationDTO
    {
        [Required]
        public int Code { get; set; }
        public SimpleClassDTO ClassDTO { get; set; }

        [Required]
        public int StudentId { get; set; }
        public SimpleStudentDTO StudentDTO { get; set; }
    }
}
