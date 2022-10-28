using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Models.DTOs
{
    public class RegistrationDTO
    {
        [Required]
        public int Code { get; set; }

        [Required]
        public int StudentId { get; set; }
    }
}
