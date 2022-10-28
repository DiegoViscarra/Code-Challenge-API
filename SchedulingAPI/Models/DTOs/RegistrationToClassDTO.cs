using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulingAPI.Models.DTOs
{
    public class RegistrationToClassDTO
    {
        [Required]
        public SimpleClassDTO SimpleClassDTO { get; set; }
        [Required]
        public IEnumerable<SimpleStudentDTO> SimpleStudentsDTOs { get; set; }
    }
}
