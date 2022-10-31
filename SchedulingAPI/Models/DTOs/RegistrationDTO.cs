using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Models.DTOs
{
    public class RegistrationDTO
    {
        [Required]
        public Guid Code { get; set; }

        [Required]
        public Guid StudentId { get; set; }
    }
}
